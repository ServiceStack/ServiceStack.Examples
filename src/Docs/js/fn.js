String.prototype.startsWith = function(str){
    return this.indexOf(str) === 0;
}

var root = window;
var count = 0;
$(document).click(function (e)
{
	var e = e || root.event, el = e.target || e.srcElement, cls = el.className;
	if (el.tagName != "A"
		|| el.href.indexOf("/docs/") == -1
		|| el.href.indexOf("/docs/default.htm") != -1
		|| el.href.indexOf("?") != -1
		|| el.href.indexOf("/category/") != -1
		|| !window.history.pushState
		) return true;

	var title = el.innerHTML;
	window.history.pushState({ title: title }, title, el.href);

	showPage(title, el.href + "?format=html.bare", el);

	e.preventDefault();
	return false;
});

window.onpopstate = function(e) {
	e = e || event;
	if (!e.state) return;
	var href = document.location + "?format=html.bare";
	showPage(e.state.title, href);
}

function showPage(title, contentUrl, el)
{
	//if (count++ == 0) return;
	
	var nextContent   = $(".next-content"),
	    activeContent = $(".active-content");
	
	$("#title").html(title);
	$('li.active').removeClass('active');

	nextContent.hide();
	activeContent.fadeOut('fast');
	activeContent.slideUp('fast');
	$.get(contentUrl, function (html)
	{
		nextContent.html(html);
		nextContent.fadeIn('fast');
		nextContent.slideDown('fast');

		nextContent.removeClass('next-content').addClass('active-content');
		activeContent.removeClass('active-content').addClass('next-content');
	});
	
	if (!el) el = $('#sidebar li>a:contains("' + title + '")')[0];	
	if (el) el.parentNode.className = 'active';
}