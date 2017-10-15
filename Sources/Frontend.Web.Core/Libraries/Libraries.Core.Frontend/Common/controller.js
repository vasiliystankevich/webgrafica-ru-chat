﻿(function ($) {
    $.Site.Libraries.Common =
    {
        stringFormat: function(str, col) {
            col = typeof col === "object" ? col : Array.prototype.slice.call(arguments, 1);

            return str.replace(/\{\{|\}\}|\{(\w+)\}/g,
                function(m, n) {
                    if (m === "{{") {
                        return "{";
                    }
                    if (m === "}}") {
                        return "}";
                    }
                    return col[n];
                });
        }
    };
})(jQuery);