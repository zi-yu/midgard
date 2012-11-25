
var timeout = null;
var tipOffTimeout = null;

function TopicTipOn(anchor, id, event)
{
	var TopicTip = document.getElementById("TopicTip");
	var targetY = document.getElementById("body").scrollTop + event.clientY + 18;
	var targetX = document.getElementById("body").scrollLeft + event.clientX + 12;
	
	TopicTip.style.left = targetX + "px";
	TopicTip.style.top = targetY + "px";
	
	var tip = 	document.getElementById(id);
	TopicTip.innerHTML = tip.innerHTML;
	TopicTip.style.display = 'block';
	if (tipOffTimeout != null)
		window.clearTimeout(tipOffTimeout);
	tipOffTimeout = window.setTimeout("TopicTipOff()", 4000, "JavaScript");
}

function TopicTipOff()
{
	var TopicTip = document.getElementById("TopicTip");
	if (tipOffTimeout != null)
		window.clearTimeout(tipOffTimeout);
	tipOffTimeout = null;				
	TopicTip.style.display = 'none';
}
