function init()
{
 var oBackground = document.getElementById("imgBackground");
 oBackground.src = "url(images/background.png)";
 startDisplaying();
}

function startDisplaying()
{
 var timer = setTimeout("DisplayQuote(1)", 1000);
}

function DisplayQuote(id)
{
 var db = new ACCESSdb("C:\\Program Files\\QuotePad\\db.mdb", {showErrors: true});
 var query = "SELECT prtfQUOTE FROM tQUOTES WHERE pID = "+id;
 var result = db.query(query, {xml:{stringOut:true}});
 GadgetShow(result);
}

function GadgetShow(text)
{
 var field = document.getElementById('quote');
 field.innerHTML = "<font color=yellow>"+text+"</font>";
}