Utils = {
    /**
    *
    */
     File:function(){
        /**
        *获取上传文件url
        *file 上传文件对象
        */
        getFileURL= function (file) {
            var url = null;
            if (window.createObjectURL != undefined) {
                url = window.createObjectURL(file);
            }
            else if (window.URL != undefined) {
                url = window.URL.createObjectURL(file);
            }
            else if (window.webkitURL != undefined) {
                url = window.webkitURL.createObjectURL(file);
            }
            return url;
        },
        getFileSize= function (file) {
            return file.size;
        }
     }
    ,

   
    /**
    *获取url指定参数
    */
    getUrlParameter: function (paramName) {
        var sValue = location.search.match(new RegExp("[\?\&]" + paramName + "=([^\&]*)(\&?)", "i"));
        return sValue ? sValue[1] : sValue;
    }
}


/**
*数组去重
*/
Array.prototype.unique = function () {
    var res = [];
    var json = {
    };
    for (var i = 0; i < this.length; i++) {
        if (typeof this[i] == 'object') {
            if (!json[JSON.stringify(this[i])]) {
                res.push(this[i]);
                json[JSON.stringify(this[i])] = 1;
            }
        } else {
            if (!json[this[i]]) {
                res.push(this[i]);
                json[this[i]] = 1;
            }
        }
    }
    return res;
}

