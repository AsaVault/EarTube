// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
        }
    })
}
AjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all').html(res.html);
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                    location.reload();

                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}


//Loader script

$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});


function AjaxSuccess() {
    location.reload();
};

function AjaxComplete() {
    $.notify("Successful", "success",
        {
            position: "bottom center",
            autoHideDelay: 5000
        }
    );
};

function AjaxLike() {
    $(".AjaxLike").notify(
        "I like this song", "success",
        {
            position: "bottom center",
            autoHideDelay: 5000
        }
    );
    setTimeout(function () {
        location.reload();
    }, 1000);

}

function AjaxDislike() {
    $(".AjaxDislike").notify(
        "I dislike this song", "success",
        {
            position: "bottom center",
            autoHideDelay: 5000
        }
    );
    setTimeout(function () {
        location.reload();
    }, 1000);
}

function LikeThis() {
    $(".LikeThis").notify(
        "I like this", "success",
        {
            position: "bottom center",
            autoHideDelay: 1000
        }
    );
    setTimeout(function () {
        location.reload();
    }, 1000);

}

function DislikeThis() {
    $(".DislikeThis").notify(
        "I dislike this", "success",
        {
            position: "bottom center",
            autoHideDelay: 1000
        }
    );
    setTimeout(function () {
        location.reload();
    }, 1000);
}

//Show more less scripts

$(document).ready(function () {
    // Configure/customize these variables.
    var showChar = 133;  // How many characters are shown by default
    var showCharTitle = 44;  // How many characters are shown by default
    var ellipsestext = "...";
    var moretext = "Read more";
    var lesstext = "Show less";


    $('.more').each(function () {
        var content = $(this).html();

        if (content.length > showChar) {

            var c = content.substr(0, showChar);
            var h = content.substr(showChar, content.length - showChar);

            var html = c + '<span class="moreellipses">' + ellipsestext + '&nbsp;</span><span class="morecontent"><span>' + h + '</span>&nbsp;&nbsp;<a href="" class="morelink link">' + moretext + '</a></span>';

            $(this).html(html);
        }

    });

    $('.short').each(function () {
        var content = $(this).html();

        if (content.length > showCharTitle) {

            var c = content.substr(0, showCharTitle);
            var h = content.substr(showCharTitle, content.length - showCharTitle);

            var html = c + '<span class="moreellipses">' + ellipsestext
                + '&nbsp;</span>';

            $(this).html(html);
        }

    });

    $(".morelink").click(function () {
        if ($(this).hasClass("less")) {
            $(this).removeClass("less");
            $(this).html(moretext);
        } else {
            $(this).addClass("less");
            $(this).html(lesstext);
        }
        $(this).parent().prev().toggle();
        $(this).prev().toggle();
        return false;
    });

    //$(".subscribeButto").click(function () {
    //    if ($(this).hasClass("btn-danger")) {
    //        $(this).removeClass("btn-danger");
    //        $(this).addClass("btn-secondary");
    //        $(this).text("Unsubscribe");
    //    } else {

    //        $(this).removeClass("btn-secondary");
    //        $(this).addClass("btn-danger");
    //        $(this).text("Subscribe");
    //    }
    //    $(this).prev().toggle();
    //    $(this).prev().toggle();
    //    return false;
    //});
});
