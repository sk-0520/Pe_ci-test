/**
最新のバージョンを抜き出す。
*/
var adSaveCreateOverWrite = 2;
var adWriteChar = 0;

var loadPath = "Pe\\PeMain\\doc\\changelog.xml";
var scriptPath = "Pe\\PeMain\\etc\\script\\changelog.js";
var stylePath  = "Pe\\PeMain\\etc\\style\\common.css";
var saveRcPath = "Changelog\\update-rc.html";
var saveReleasePath = "Changelog\\update-release.html";

var typeMap = {
	'features':  '機能',
	'fixes':     '修正',
	'developer': '開発',
	'note':      'メモ'
};

var xml = WScript.CreateObject('MSXML.DOMDocument');
xml.load(loadPath);

var updateVersions = [
	{
		type: 'rc',
		path: saveRcPath
	},
	{
		type: 'release',
		path: saveReleasePath
	}
];

function isRc(update)
{
	return update.type == 'rc';
}

function isRcElement(node)
{
	return node.getAttribute('type') == 'rc';
}

function createStream()
{
	var stream = WScript.CreateObject('ADODB.Stream');
	stream.Mode = 3;
	stream.Type = 2;
	stream.Charset = 'UTF-8';
	stream.Open();

	return stream;
}

function writeLine(stream, s)
{
	stream.WriteText(s);
	stream.WriteText("\r\n");
}

function writeHead(stream)
{
	var scriptStream = createStream();
	scriptStream.LoadFromFile(scriptPath);
	var scriptText = scriptStream.ReadText;
	
	var styleStream = createStream();
	styleStream.LoadFromFile(stylePath);
	var styleText = styleStream.ReadText;
	
	writeLine(stream, "<!DOCTYPE html>\r\n");
	writeLine(stream, '<html>');
	writeLine(stream, '<head>');
	writeLine(stream, '<meta charset="utf-8">');
	writeLine(stream, '<title>Pe Update: 最新バージョン更新情報</title>');
	writeLine(stream, '<script>');
	writeLine(stream, scriptText);
	writeLine(stream, '</script>');
	writeLine(stream, '<style>');
	writeLine(stream, styleText);
	writeLine(stream, '</style>');
	writeLine(stream, '</head>');
	writeLine(stream, '<body>');
}

function writeFoot(stream)
{
	writeLine(stream, '</body>');
	writeLine(stream, '</html>');
}

function versionToText(element)
{
	var version = element.getAttribute('version');
	var date    = element.getAttribute('date');
	if(isRcElement(element)) {
		return date + ', ' + version + ' <em>RC版</em>';
	} else {
		return date + ', ' + version;
	}
}

function writeBlock(stream, element)
{
	writeLine(stream, '<dl class="changelog">');
	var groups = element.getElementsByTagName('group');
	for(var i = 0; i < groups.length; i++) {
		var group = groups[i];
		var type = group.getAttribute('type');

		var notes = group.getElementsByTagName('note');
		var list = [];
		for(var noteIndex = 0; noteIndex < notes.length; noteIndex++) {
			//var s = notes[noteIndex].text;
			var note = notes[noteIndex];
			if(note.text.length) {
				list.push({
					className: note.getElementsByTagName('message')[0].getAttribute('class'),
					body: note.getElementsByTagName('message')[0].text,
					comments: function() {
						var result = [];
						var commentElements = note.getElementsByTagName('comment');
						if(commentElements) {
							for(var commentIndex = 0; commentIndex < commentElements.length; commentIndex++) {
								var comment = commentElements[commentIndex].text;
								if(comment.length) {
									result.push(comment);
								}
							}
						}
						return result;
					}(),
					rev: function() {
						var rev = note.getAttribute('rev');
						if(rev) {
							return '<a class="rev">' + rev + '</a>'
						}
						return '';
					}()
				});
			}
		}
		if(list.length) {
			writeLine(stream, "<dt class='" + type + "'>" + typeMap[type] + '</dt>');
			writeLine(stream, "<dd class='" + type + "'>");
			writeLine(stream, '<ul>');
			
			for(var listIndex = 0; listIndex < list.length; listIndex++) {
				//var text = list[listIndex].replace(/#([0-9]+)/g, '<a href="' + issueLink + '$1">#$1</a>');
				var item = list[listIndex];
				
				var listHead;
				if(item.className) {
					listHead = '<li class="' + item.className + '">';
				} else {
					listHead = '<li>';
				}
				writeLine(stream, listHead);
				writeLine(stream, item.body + '。' + item.rev);
				if(item.comments && item.comments.length) {
					writeLine(stream, '<ul class="comment">');
					for(var commentIndex = 0; commentIndex < item.comments.length; commentIndex++) {
						writeLine(stream, '<li>' + item.comments[commentIndex] + '。</li>');
					}
					writeLine(stream, '</ul>');
				}
				writeLine(stream, '</li>');
			}
			
			writeLine(stream, '</ul>');
			writeLine(stream, '</dd>');
		}
	}
	
	writeLine(stream, '</dl>');
}

//個別履歴出力
for(var i = 0; i < updateVersions.length; i++) {
	var update = updateVersions[i];

	var stream = createStream();
	writeHead(stream);

	var log = null;
	var logs = xml.getElementsByTagName('log');
	for(var j = 0; j < logs.length; j++) {
		if(isRcElement(logs[j])) {
			if(isRc(update)) {
				log = logs[j];
				break;
			}
		} else if(!isRc(update)) {
			log = logs[j];
			break;
		}
	}
	if(!log) {
		continue;
	}

	var title = versionToText(log);
	writeLine(stream, '<h2>' + title + '</h2>');

	writeBlock(stream, log);
	
	writeFoot(stream);

	stream.SaveToFile(update.path, adSaveCreateOverWrite);
	stream.Close();
}



