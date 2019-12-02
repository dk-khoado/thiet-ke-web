var express = require('express')
var route = express.Router();
var md5 = require('md5');

var mUser = require('../models/users');
var mHistoty = require("../models/history_login");
//register user
route.post('/register', function (req, res) {
    var body = req.body;
    if (body.password) {
        body.password = md5(body.password);
    }
    console.log(body.username + " has Register!");
    mUser.create(body, function (err, result) {
        if (err) {
            var request = {
                err: true,
                email: 0,
                username: 0
            }
            console.log(err);
            if (err.keyValue.email) {
                request.email = 1;
            }
            if (err.keyValue.username) {
                request.username = 1;
            }
            res.send(request);
        } else {
            console.log(body.username + " has Register!");
            res.send(result);
        }
    })
});

route.post("/login", function (req, res) {
    var body = req.body;
    var data = null;
    if (body.password) {
        body.password = md5(body.password);
    }
    mUser.find({
        username: body.username,
        password: body.password
    }, function (err, response) {
        //lấy dữ liệu
        if (err) {
            console.log("loi: " + err);
            res.send({
                result: 0,
                message: "Login Fail!"
            });
        } else {
            if (response.length > 0) {
                console.log(body.username + " has login!");
                var time_login = new Date().getTime();
                mHistoty.create({
                    username: body.username,
                    email: response[0].email,
                    date_login: time_login
                },function(err,result){
                    console.log(body.username + " login in "+ time_login);
                })
                res.send({
                    result: 1,
                    message: "Login Success!",
                    data:  response[0],
                    date: time_login
                });
            } else {
                res.send({
                    result: 0,
                    message: "Login Fail!"
                });
            }
        }
    });
})

route.post("/update", function (req, res) {
    var body = req.body;
    if (body.password) {
        body.password = md5(body.password);
    }
    mUser.updateOne({
        _id: req.param("id", 0)
    }, {
        name: body.name,
        password: body.password
    }, function (err, raw) {
        if (err) {
            res.status(404).send("lỗi")
        } else {
            res.send(raw);
        }
    })
})
module.exports = route;