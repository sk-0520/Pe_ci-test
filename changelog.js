/**
最新のバージョンを抜き出す。
*/
var adSaveCreateOverWrite = 2;
var adWriteChar = 0;

var loadPath = "Pe\\PeMain\\doc\\changelog.xml";
var saveRcPath = "Update\\update-rc.html";
var saveReleasePath = "Update\\update-release.html";
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


for(var i = 0; i < updateVersions.length; i++) {
	var update = updateVersions[i];
	
	var stream = WScript.CreateObject('ADODB.Stream');
	stream.Mode = 3;
	stream.Type = 2;
	stream.Charset = 'UTF-8';
	stream.Open();

	stream.WriteText("<!DOCTYPE html>\r\n");
	stream.WriteText('<html>');
	stream.WriteText('<head>');
	stream.WriteText('<meta charset="utf-8">');
	stream.WriteText('<title>Pe Update: Change log</title>');
	stream.WriteText('<style>');
	stream.WriteText('body');
	stream.WriteText('{');
	stream.WriteText('	font-family: "Meiryo UI";');
	stream.WriteText('}');

	stream.WriteText('h1');
	stream.WriteText('{');
	stream.WriteText('	border-bottom: 3px double #888;');
	stream.WriteText('}');

	stream.WriteText('dt.features');
	stream.WriteText('{');
	stream.WriteText('}');

	stream.WriteText('dt.fixes');
	stream.WriteText('{');
	stream.WriteText('}');

	stream.WriteText('dt.developer');
	stream.WriteText('{');
	stream.WriteText('}');

	stream.WriteText('dt.note');
	stream.WriteText('{');
	stream.WriteText('}');

	stream.WriteText('dd');
	stream.WriteText('{');
	stream.WriteText('	margin-left: 0;');
	stream.WriteText('}');

	stream.WriteText('</style>');
	stream.WriteText('</head>');
	stream.WriteText('<body>');

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
	
	var version = log.getAttribute('version');
	var date    = log.getAttribute('date');
	if(isRc(update)) {
		stream.WriteText('<h1>' + date + ', ' + version + ' <strong>RC版</strong></h1>');
	} else {
		stream.WriteText('<h1>' + date + ', ' + version + '</h1>');
	}

	stream.WriteText('<dl>');
	var list = log.getElementsByTagName('ul');
	for(var j = 0; j < list.length; j++) {
		var type = list[j].getAttribute('type');

		var items = list[j].getElementsByTagName('li');
		var comment = [];
		for(var k = 0; k < items.length; k++) {
			var s = items[k].text;
			if(s.length) {
				comment.push(s);
			}
		}
		if(comment.length) {
			stream.WriteText("<dt class='" + type + "'>" + typeMap[type] + '</dt>');
			stream.WriteText('<dd>');
			stream.WriteText('<ul>');
			
			for(var k = 0; k < comment.length; k++) {
				stream.WriteText('<li>' + comment[k] + '</li>');
			}
			
			stream.WriteText('</ul>');
			stream.WriteText('</dd>');
		}
	}
	stream.WriteText('</dl>');

	stream.WriteText('</body>');
	stream.WriteText('</html>');

	stream.SaveToFile(update.path, adSaveCreateOverWrite);
	stream.Close();
}

