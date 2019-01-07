/*
url:请求地址
data:请求参数的对象
type:请求类型，post或者get
datatype:请求使用的数据类型
success:成功执行的函数
error:失败执行的函数
*/
function doAjax(url, data, type, datatype, success, async, error) {
    $.ajax({
        url: url,
        data: data,
        type: type,
        dataType: datatype,
        success: function (result) { success(result) },
        async: async,
        error: function (XMLHttpRequest, Msg) {
            if (error) {
                error(XMLHttpRequest, Msg)
            }
            else {
                layer.msg(Msg);

            }

        }
    });
}

function InitAjax(url, data, success, error) {
    var type = "post";
    var dataType = "json";
    $.ajax({
        url: url,
        data: data,
        type: type,
        dataType: datatype,
        success: function (result) { success(result) },
        async: true,
        error: function (XMLHttpRequest, Msg) {
            if (error) {
                error(XMLHttpRequest, Msg)
            }
            else {
                layer.msg(Msg);

            }

        }
    });
}