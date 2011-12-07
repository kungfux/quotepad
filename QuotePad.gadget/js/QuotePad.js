//////////////////////
// Global variables //
//////////////////////

var TimeToUpdate = 5000;   // How often random quote should be updated
var pathToDb = "";         // Path to database

var BufferSize = 0.99;     // BufferSize in persents
var BufferSizeLimit = 200; // Max allowed buffer size
var IsReady = false;       // Is Buffer initialized?
var Buffer;                // Buffer
var Max;                   // Max ID of quote in DB
var RandomValue;           // Random number
var RandomQuote;           // Random Quote
var MaxFailedAttempts = 20;// Max failed queries to reinit random

//////////////////////
// System functions //
//////////////////////

function ReadRegistry(key) // Read key value from Registry
{
	var wsh = new ActiveXObject("WScript.Shell");
	return wsh.RegRead(key);
}

///////////////////////////////
// Internal Gadget functions //
///////////////////////////////

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

////////////////////////
// DataBase functions //
////////////////////////

function PerformQuery(query, tagName)
{
	var db = new ACCESSdb(pathToDb, {showErrors: true});
	var result = db.query(query, {xml:{stringOut:true}});
	if (result)
	{
		var pos1 = result.indexOf("<"+tagName) + tagName.length + 2;
		var pos2 = result.indexOf("</"+tagName);
		//alert("Query: "+query+"         Full stack: "+result+"     Clear stack:"+result.substr(pos1, pos2-pos1));
		return result.substr(pos1, pos2-pos1);
	}
	else return false;
}

function IsDatabaseAccessable() // Check connection to database
{
	var db = new ACCESSdb(pathToDb, {showErrors: true});
	if (PerformQuery("SELECT COUNT(*) FROM tQUOTES", "Expr1000"))
	return true;
	else return false;
}

function GetQuotesCount() // Get count of quotes in db
{
	if (IsDatabaseAccessable())
	return PerformQuery("SELECT COUNT(*) FROM tQUOTES", "Expr1000");
}

function Quote_FindByID(id) // Get quote by quote ID
{
	return PerformQuery("SELECT ptxtQUOTE FROM tQUOTES WHERE pID = "+id, "ptxtQUOTE");
}

////////////////////////
// QuotePad functions //
////////////////////////

function rnd(num) // get random number
{
	return Math.floor(Math.random()*num) + 1;
}

function RandomInit() // init random functionality
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
		var FailsWatcher = 0;
		while (RandomQuote == false)
		{
			Random_FindNext();
			FailsWatcher++;
			if (FailsWatcher > MaxFailedAttempts) 
			{
				RandomQuote = "Загрузка цитат...";
				IsReady = false;
				break;
			}
		}
		return RandomQuote;
	}
	else return '<font color=red>Нет цитат для отображения.</font><br><a href="#" onClick="ReInitPlease()">Повторить</a>';
}

function ReInitPlease()
{
	IsReady = false;
}

//////////////////
// MAIN SECTION //
//////////////////

function DisplayText(text)
{
	var field = document.getElementById('quote');
	field.innerHTML = "<font color=green>"+text+"</font>";
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
	else 
	{
		DisplayError("Не удалось прочесть конфигурацию.");
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