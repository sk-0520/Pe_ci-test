/**
最新のバージョンを抜き出す。
*/
var adSaveCreateOverWrite = 2;
var adWriteChar = 0;

//var targetName = 'PE_BROWSER';
//var issueLink = 'https://bitbucket.org/sk_0520/pe/issue/';
var loadPath = "Pe\\PeMain\\doc\\changelog.xml";
var scriptPath = "Pe\\PeMain\\etc\\script\\changelog.js";
var stylePath  = "Pe\\PeMain\\etc\\style\\common.css";
var saveRcPath = "Update\\update-rc.html";
var saveReleasePath = "Update\\update-release.html";
//var saveChangelogPath = "Pe\\PeMain\\doc\\changelog.html";
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

function writeHead(stream)
{
	var scriptStream = createStream();
	scriptStream.LoadFromFile(scriptPath);
	var scriptText = scriptStream.ReadText;
	
	var styleStream = createStream();
	styleStream.LoadFromFile(stylePath);
	var styleText = styleStream.ReadText;
	
	stream.WriteText("<!DOCTYPE html>\r\n");
	stream.WriteText('<html>');
	stream.WriteText('<head>');
	stream.WriteText('<meta charset="utf-8">');
	stream.WriteText('<title>Pe Update: 最新バージョン更新情報</title>');
	stream.WriteText('<script>');
	stream.WriteText(scriptText);
	stream.WriteText('</script>');
	stream.WriteText('<style>');
	stream.WriteText(styleText);
	stream.WriteText('</style>');
	stream.WriteText('</head>');
	stream.WriteText('<body>');
}

function writeFoot()
{
	stream.WriteText('</body>');
	stream.WriteText('</html>');
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
	stream.WriteText('<dl>');
	var list = element.getElementsByTagName('ul');
	for(var i = 0; i < list.length; i++) {
		var type = list[i].getAttribute('type');

		var items = list[i].getElementsByTagName('li');
		var comment = [];
		for(var j = 0; j < items.length; j++) {
			var s = items[j].text;
			if(s.length) {
				comment.push(s);
			}
		}
		if(comment.length) {
			stream.WriteText("<dt class='" + type + "'>" + typeMap[type] + '</dt>');
			stream.WriteText('<dd>');
			stream.WriteText('<ul>');
			
			for(var j = 0; j < comment.length; j++) {
				//var text = comment[j].replace(/#([0-9]+)/g, '<a href="' + issueLink + '$1">#$1</a>');
				var text = comment[j];
				stream.WriteText('<li>' + text + '。</li>');
			}
			
			stream.WriteText('</ul>');
			stream.WriteText('</dd>');
		}
	}
	
	stream.WriteText('</dl>');
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
	stream.WriteText('<h1>' + title + '</h1>');

	writeBlock(stream, log);
	
	writeFoot(stream);

	stream.SaveToFile(update.path, adSaveCreateOverWrite);
	stream.Close();
}

/*
// 全体履歴出力
var stream = createStream();
writeHead(stream);

stream.WriteText('<h1>Pe Changelog</h1>');
var logs = xml.getElementsByTagName('log');
for(var i = 0; i < logs.length; i++) {
	var log = logs[i];
	var title = versionToText(log);
	stream.WriteText('<h2>' + title + '</h2>');
	writeBlock(stream, log);
}
writeFoot(stream);
stream.SaveToFile(saveChangelogPath, adSaveCreateOverWrite);
stream.Close();
*/

