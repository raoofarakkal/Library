(function ($) {

    $.event.special.textchange = {

        setup: function (data, namespaces) {
            $(this).data('lastValue', this.contentEditable === 'true' ? $(this).html() : $(this).val());
            $(this).bind('keyup.textchange', $.event.special.textchange.handler);
            $(this).bind('cut.textchange paste.textchange input.textchange', $.event.special.textchange.delayedHandler);
        },

        teardown: function (namespaces) {
            $(this).unbind('.textchange');
        },

        handler: function (event) {
            $.event.special.textchange.triggerIfChanged($(this));
        },

        delayedHandler: function (event) {
            var element = $(this);
            setTimeout(function () {
                $.event.special.textchange.triggerIfChanged(element);
            }, 25);
        },

        triggerIfChanged: function (element) {
            var current = element[0].contentEditable === 'true' ? element.html() : element.val();
            if (current !== element.data('lastValue')) {
                element.trigger('textchange', element.data('lastValue'));
                element.data('lastValue', current);
            }
        }
    };

    $.event.special.hastext = {

        setup: function (data, namespaces) {
            $(this).bind('textchange', $.event.special.hastext.handler);
        },

        teardown: function (namespaces) {
            $(this).unbind('textchange', $.event.special.hastext.handler);
        },

        handler: function (event, lastValue) {
            if ((lastValue === '') && lastValue !== $(this).val()) {
                $(this).trigger('hastext');
            }
        }
    };

    $.event.special.notext = {

        setup: function (data, namespaces) {
            $(this).bind('textchange', $.event.special.notext.handler);
        },

        teardown: function (namespaces) {
            $(this).unbind('textchange', $.event.special.notext.handler);
        },

        handler: function (event, lastValue) {
            if ($(this).val() === '' && $(this).val() !== lastValue) {
                $(this).trigger('notext');
            }
        }
    };

})(jQuery);



(function () {

    var fieldSelection = {

        getSelection: function () {

            var e = (this.jquery) ? this[0] : this;

            return (

            /* mozilla / dom 3.0 */
				('selectionStart' in e && function () {
				    var l = e.selectionEnd - e.selectionStart;
				    return { start: e.selectionStart, end: e.selectionEnd, length: l, text: e.value.substr(e.selectionStart, l) };
				}) ||

            /* exploder */
				(document.selection && function () {

				    e.focus();

				    var r = document.selection.createRange();
				    if (r === null) {
				        return { start: 0, end: e.value.length, length: 0 }
				    }

				    var re = e.createTextRange();
				    var rc = re.duplicate();
				    re.moveToBookmark(r.getBookmark());
				    rc.setEndPoint('EndToStart', re);

				    return { start: rc.text.length, end: rc.text.length + r.text.length, length: r.text.length, text: r.text };
				}) ||

            /* browser not supported */
				function () { return null; }

			)();

        },

        replaceSelection: function () {

            var e = (typeof this.id == 'function') ? this.get(0) : this;
            var text = arguments[0] || '';

            return (

            /* mozilla / dom 3.0 */
				('selectionStart' in e && function () {
				    e.value = e.value.substr(0, e.selectionStart) + text + e.value.substr(e.selectionEnd, e.value.length);
				    return this;
				}) ||

            /* exploder */
				(document.selection && function () {
				    e.focus();
				    document.selection.createRange().text = text;
				    return this;
				}) ||

            /* browser not supported */
				function () {
				    e.value += text;
				    return jQuery(e);
				}

			)();

        }

    };

    jQuery.each(fieldSelection, function (i) { jQuery.fn[i] = this; });

})();


function GetWordCount(pString) {
    try {
        var mStr = pString.replace(/\s+/g, ' ');
        while (mStr.substring(0, 1) == ' ')
            mStr = mStr.substring(1);
        while (mStr.substring(mStr.length - 2, mStr.length - 1) == ' ')
            mStr = mStr.substring(0, mStr.length - 1);
        var mStr2 = mStr.split(' ');
        return mStr2.length;
    } catch (e) { }
}

function Wcc(pText, pTarget, pMax) {
    try {
        var mW = GetWordCount(pText);
        var mC = pText.length;
        if (pMax != null) {
            var mR = pMax - mC;
            $(pTarget).html('W: ' + mW + '  ||  C: ' + mC + '  ||  R: <span ' + ((mR < 0) ? 'style="color:red"' : '') + '>' + mR + '</span>');
        } else {
            $(pTarget).html('W: ' + mW + '  ||  C: ' + mC);
        }
    } catch (e) { }
}
