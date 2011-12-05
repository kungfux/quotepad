//
// Global variables
//

var TimeToUpdate = 5000; // How often random quote should be updated
var pathToDb = ""; // Path to database










//
// System functions
//

// Read key value from Registry
function ReadRegistry(key)
{
	var wsh = new ActiveXObject("WScript.Shell");
	return wsh.RegRead(key);
}











//
// Internal Gadget functions
// 

function InitGadget()
{
	// Set settings page
	/*System.Gadget.settingsUI = "settings.html";
	System.Gadget.onSettingsClosed = settingsClosed;*/
	
	// Get path to database from registry
	pathToDb = ReadRegistry("HKLM\\Software\\ItWorksTeam\\QuotePad\\Database");
	if (pathToDb == "")
	{
		DisplayError("Не удаётся найти базу данных!");
		return false;
	}
	return true; // init done successfully
}

function settingsClosed(e) 
{   
	if (e.closeAction == e.Action.commit) 
	{     
		//System.Gadget.Settings.writeString('uri', 'http://designformasters.info/');
		//var uri = System.Gadget.Settings.readString('uri');  
	} 
}









//
// DataBase functions
//

function PerformQuery(query, tagName)
{
	var db = new ACCESSdb(pathToDb, {showErrors: true});
	var result = db.query(query, {xml:{stringOut:true}});
	var pos1 = result.indexOf("<"+tagName) + tagName.length + 2;
	var pos2 = result.indexOf("</"+tagName);
	//alert("Query: "+query+"         Full stack: "+result+"     Clear stack:"+result.substr(pos1, pos2-pos1));
	return result.substr(pos1, pos2-pos1);
}

function IsDatabaseAccessable()
{
	var db = new ACCESSdb(pathToDb, {showErrors: true});
	if (PerformQuery("SELECT COUNT(*) FROM tQUOTES", "Expr1000"))
	return true;
	else return false;
}

function GetQuotesCount()
{
	if (IsDatabaseAccessable())
	return PerformQuery("SELECT COUNT(*) FROM tQUOTES", "Expr1000");
}

function Quote_FindByID(id)
{
	return PerformQuery("SELECT prtfQUOTE FROM tQUOTES WHERE pID = "+id, "prtfQUOTE");
}
//
// QuotePad functions
// 
var BufferSize = 0.99;
var BufferSizeLimit = 200;
var IsReady = false;
var Buffer;
var Max;
var RandomValue;
var RandomQuote;

function rnd(num)
{
	return Math.floor(Math.random()*num);
	//return Math.round(num*Math.random()+0.50);
}

function RandomInit()
{
	var RecordsCount = GetQuotesCount();
	if (RecordsCount * BufferSize < BufferSizeLimit)
	{
		Buffer = new Array(Math.floor(RecordsCount * BufferSize));
	}
	else
	{
		Buffer = new Array(BufferSizeLimit);
	}
	if (RecordsCount > 0)
	{
		Max = PerformQuery("SELECT MAX(pID) FROM tQUOTES", "Expr1000")
		for (var a = 0; a < Buffer.length; a++)
		{
			Buffer[a] = 0;
		}
	}
	else Max = 0;
}

function UpdateBuffer(value)
{
	if (Buffer.length > 0)
	{
		for (var a = 1; a < Buffer.length; a++)
		{
			Buffer[a - 1] = Buffer[a];
		}
	Buffer[Buffer.length - 1] = value;
	}
}

function IsValueInBuffer(value)
{
	for (var a = 1; a < Buffer.length; a++)
	{
		if (Buffer[a] == value) return true;
	}
	return false;
}

function Random_FindNext()
{
	RandomValue = rnd(Max);
	while (IsValueInBuffer(RandomValue))
	{
		RandomValue = rnd(Max);
	}
	UpdateBuffer(RandomValue);
	RandomQuote = Quote_FindByID(RandomValue);
}

function RandomRead()
{
	if (!IsReady)
	{
		RandomInit();
		IsReady = !IsReady;
	}
	if (Max > 0)
	{
		RandomQuote = false;
		while (RandomQuote == false)
		{
			Random_FindNext();
		}
		return RandomQuote;
	}
	else return "<font color=red>Нет цитат для отображения.</font>";
}







//
// MAIN SECTION
//

function DisplayText(text)
{
	var field = document.getElementById('quote');
	field.innerHTML = "<font color=yellow>"+text+"</font>";
}

function DisplayError(text)
{
	var field = document.getElementById('quote');
	field.innerHTML = "<font color=red>"+text+"</font>";
}

function StartGadget()
{
	if (InitGadget())
	{
		startDisplaying();
	}
}

function UpdateByTime()
{
	setTimeout(UpdateByTime, TimeToUpdate);
	DisplayText(RandomRead());
}

function startDisplaying()
{
	if (IsDatabaseAccessable())
	{	
		UpdateByTime();
	}
	else
	{
		DisplayError("База данных не найдена либо нет доступа к ActiveX.");
	}
}