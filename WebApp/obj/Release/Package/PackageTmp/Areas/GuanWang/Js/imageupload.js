//文件上传
function doUpload(filename) {
    // 上传方法
    $.upload({
        // 上传地址
        url: '/GuanWang/ManagerHome/Upload',
        // 文件域名字
        fileName: 'filedata',
        // 其他表单数据
        params: { action: filename },
        // 上传完成后, 返回json, text
        dataType: 'json',
        // 上传之前回调,return true表示可继续上传
        onSend: function () {
            return true;
        },
        // 上传之后回调
        onComplate: function (serverData) {
            var jsondata = $.parseJSON(serverData);
            if (jsondata.status == 0) {
                $("#txtimagesrc").attr("src", jsondata.data);
                $("#txtimagevalue").attr("value", jsondata.data);
            } else {
                alert(jsondata.msg);
            }
        }
    });
}