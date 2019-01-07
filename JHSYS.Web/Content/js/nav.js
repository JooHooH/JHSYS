$(function () {
    //初始化菜单栏
    InitMenu();
    //$(".nav").height($(window).height - 50);
    $("#westInfo").find('.nav').height($(window).height() - 50);
    $("#centerInfo").find('.mainContent').height($(window).height() - 50);
    $("#centerInfo").find('.mainContent').width($(window).width() - 220);
    $(window).resize(function (e) {
        $("#westInfo").find('.nav').height($(window).height() - 50);
        $("#centerInfo").find('.mainContent').height($(window).height() - 50);
        $("#centerInfo").find('.mainContent').width($(window).width() - 220);
    });
    
    $('.nav-item > a').on('click', function () {
        if (!$('.nav').hasClass('nav-mini')) {
            if ($(this).next().css('display') == "none") {
                $('.nav-item').children('ul').slideUp(300);
                $(this).next('ul').slideDown(300);
                $(this).parent('li').addClass('nav-show').siblings('li').removeClass('nav-show');
            } else {
                $(this).next('ul').slideUp(300); $('.nav-item.nav-show').removeClass('nav-show');
            }
        }
    });

    $('#mini').on('click', function () {
        if (!$('.nav').hasClass('nav-mini')) {
            $('.nav-item.nav-show').removeClass('nav-show');
            $('.nav-item').children('ul').removeAttr('style');
            $('.nav').addClass('nav-mini');
            $("#centerInfo").find('.mainContent').width($(window).width() - 60);
            $("#centerInfo").find('.mainContent').height($(window).height() - 50);
            $("#centerInfo").find('.mainContent').css("left",60);
        } else {
            $("#centerInfo").find('.mainContent').width($(window).width() - 220);
            $("#centerInfo").find('.mainContent').height($(window).height() - 50);
            $("#centerInfo").find('.mainContent').css("left", 220);
            $('.nav').removeClass('nav-mini');
        }
    });
});

function InitMenu() {
    var type = "post";
    var dataType = "json";
    var url = "/Home/Menu";
    var data = {};
    var successs = function (result) {
        var data = result.data;
        Sysmenu(data);
    };
    doAjax(url, data, type, dataType, successs, false);
}

function Sysmenu(result) {
    var data = JSON.parse(result);
    //console.log(data);
    var _html = "";
    $.each(data, function (i) {
        var row = data[i];
        //console.log("11"+row);
        if (row.ParentID == "0") {
            _html += '<li class="nav-item">';
            _html += '<a data-id="' + row.Id + '" href="#"><i class="' + row.MenuIcon + '"></i><span>' + row.MenuName + '</span><i class="fa fa-caret-right a-right"></i></a>';
            var childNodes = row.ChildNodes;
            if (childNodes.length > 0) {
                console.log("22");
                _html += '<ul class="submenu">';
                $.each(childNodes, function (i) {
                    var subrow = childNodes[i];
                    _html += '<li>';
                    _html += '<a class="menuItem" data-id="' + subrow.Id + '" href="' + subrow.MenuUrl + '" data-index="' + subrow.MenuSort + '"><span>' + subrow.MenuName + '</span></a>';
                    _html += '</li>';
                });
                _html += '</ul>';
            }
            _html += '</li>';
        }
    });
    //console.log(_html);
    $(".nav ul").prepend(_html);
}
