(function ($) {
    $.Site.Controllers.Home.Index =
    {
        appendContent: function(element, format, data) {
            var content = $.Site.Libraries.Common.stringFormat(format, data);
            element.append(content);
        }
    };
    $.Site.Libraries.WebApi.Common.ExecuteAction(
        {
            uri: $("#api-messages-getmessages").attr("href"),
            data: { CountLastMessages: 10 },
            LongPooling: true,
            LongPoolingTime: 3000,
            onSuccess: function (response) {
                var contentFormatUserName = "<p>{0}: </p>";
                var contentFormatTimeMessage = "<time>{0}</time>";
                var contentFormatLineMessage = "<p>{0}</p>";
                var userMessageNodes = [];
                $(response.Messages).each(function (index, message) {
                    var node = (message.IsSelf === true) ? $(".self-hidden").clone() : $(".other-hidden").clone();
                    node.removeClass("hidden other-hidden");
                    node.removeClass("hidden self-hidden");
                    var userMessage = node.find(".msg");
                    var userName = (message.IsSelf === true) ? "Вы" : message.UserName;
                    $.Site.Controllers.Home.Index.appendContent(userMessage, contentFormatUserName, userName);
                    if (message.Content != null) {
                        var lines = message.Content.split("\n");
                        $(lines).each(function(indexLine, line) {
                            $.Site.Controllers.Home.Index.appendContent(userMessage, contentFormatLineMessage, line);
                        });
                    }
                    $.Site.Controllers.Home.Index.appendContent(userMessage, contentFormatTimeMessage, message.TimeMessage);
                    userMessageNodes.push(node);
                });
                var chat = $(".chat");
                chat.empty();
                $(userMessageNodes).each(function(index, message) {
                    chat.append(message);
                });
                var chatHeight = $(".chat").height() + 15;
                $("html,body").animate({ "scrollTop": chatHeight }, 500);
            },
            onException: function(response) { alert(response.Status.Message); }
        });
    $("#button-send").on("click",
        function() {
            $.Site.Libraries.WebApi.Common.ExecuteAction(
                {
                    uri: $("#api-messages-sendmessage").attr("href"),
                    data: { Message: $("#textarea-message").val() },
                    onSuccess: function() { $("#textarea-message").val("") },
                    onException: function(response) { alert(response.Status.Message); }
                });
        });
})(jQuery);