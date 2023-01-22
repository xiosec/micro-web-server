var ADDRES = 'http://127.0.0.1:8080/';
var Main = /** @class */ (function () {
    function Main(url) {
        var _this = this;
        this.url = url;
        var btnSearch = document.getElementById("btn-search");
        var txtSearch = document.getElementById("txt-search");
        btnSearch.onclick = function (e) {
            _this.search(txtSearch === null || txtSearch === void 0 ? void 0 : txtSearch.value);
        };
        var btnReload = document.getElementById("btn-reload");
        btnReload.onclick = function (e) {
            _this.get().then(function (data) {
                _this.show(data);
            });
        };
        var txtFirstname = document.getElementById("txt-first");
        var txtLastname = document.getElementById("txt-last");
        var txtAddress = document.getElementById("txt-address");
        var txtEmail = document.getElementById("txt-email");
        var btnCreate = document.getElementById("btn-create");
        btnCreate.onclick = function (e) {
            if (txtFirstname.value.length > 1
                && txtLastname.value.length > 1
                && txtAddress.value.length > 1
                && txtEmail.value.length > 1) {
                var data = {
                    Firstname: txtFirstname.value,
                    Lastname: txtLastname.value,
                    Address: txtAddress.value,
                    Email: txtEmail.value
                };
                _this.create(data);
            }
        };
    }
    Main.prototype.get = function () {
        return fetch(this.url)
            .then(function (response) { return response.json(); });
    };
    Main.prototype.delete = function (id) {
        fetch(ADDRES + "delete?id=" + id).then(function (value) {
            value.json().then(function (data) {
                var _a;
                if (data.status) {
                    (_a = document.getElementById("row" + id)) === null || _a === void 0 ? void 0 : _a.remove();
                }
            });
        });
    };
    Main.prototype.add = function (data) {
        var _this = this;
        var table = document.getElementById("tb_main");
        var len = table.rows.length;
        var row = table.insertRow(len);
        row.setAttribute('id', "row" + data.Id);
        row.insertCell(0).innerHTML = len;
        row.insertCell(1).innerHTML = data.Firstname;
        row.insertCell(2).innerHTML = data.Lastname;
        row.insertCell(3).innerHTML = data.Address;
        row.insertCell(4).innerHTML = data.Email;
        row.insertCell(5).innerHTML = data.time;
        var btn = document.createElement('button');
        btn.textContent = 'Delete';
        btn.setAttribute('class', 'btn btn-danger');
        btn.onclick = function (e) {
            _this.delete(data.Id);
        };
        row.insertCell(6).appendChild(btn);
    };
    Main.prototype.clearTable = function () {
        var table = document.getElementById("tb_main");
        //clear old item
        table.innerHTML = null;
        var row = table.insertRow(0);
        row.insertCell(0).innerHTML = "#";
        row.insertCell(1).innerHTML = "First";
        row.insertCell(2).innerHTML = "Last";
        row.insertCell(3).innerHTML = "Address";
        row.insertCell(4).innerHTML = "Email";
        row.insertCell(5).innerHTML = "Time";
        row.insertCell(6).innerHTML = "option";
    };
    Main.prototype.show = function (data) {
        var _this = this;
        var rowCount = 1;
        var table = document.getElementById("tb_main");
        this.clearTable();
        data.forEach(function (element) {
            _this.add(element);
        });
    };
    Main.prototype.search = function (name) {
        var _this = this;
        if (name.length > 1) {
            fetch(this.url + "find?name=" + name)
                .then(function (response) { return response.json().then(function (data) { return _this.show(data); }); });
        }
    };
    Main.prototype.create = function (data) {
        var _this = this;
        fetch(this.url + "create", {
            method: "POST",
            body: JSON.stringify(data)
        }).then(function (value) {
            value.json().then(function (data) {
                _this.add(data);
            });
        });
    };
    return Main;
}());
var main = new Main(ADDRES);
main.get().then(function (value) {
    main.show(value);
});
export {};
