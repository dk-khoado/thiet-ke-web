'use strict';

var express = require('express');
var app = express();
var http = require('http').createServer(app);
var io = require('socket.io')(http);
var cookie = require("cookie-parser");
var port = process.env.port || 3000;
http.listen(port);

var mongodb = require('mongoose');

var connectString = "mongodb+srv://admin:khoa123@cluster0-yvrcs.mongodb.net/chat?retryWrites=true&w=majority";

mongodb.connect(connectString, {
    useNewUrlParser: true,
    useUnifiedTopology: true,
    useCreateIndex: true
}).then(function(res){
    console.log("db connect!");
}).catch(function(err){
    console.log(err);
});

var api = require("./router/api");
var home = require("./router/home");
//static
app.use(express.static(__dirname + "/public/"));

//settup express
app.use(express.json());
app.use(express.urlencoded({
    extended: false
}));
app.engine('html', require('ejs').renderFile);

app.use('/', home)
app.use('/api', api)
app.use(cookie());

//chat
var roomID = 0;
var inRoom = [];
io.on("connection", function (socket) {
    console.log("a client connection");
    socket.on("chat", function (data) {
        data.id = socket.id;
        io.sockets.in(data.roomID).emit("chat", data);
        console.log(socket.id + " send a message");
    })
    //create room and waitting
    socket.on('create', function (data) {
        let hasFound = false;
        let created = false;
        if (inRoom.length < 1) {
            roomID++;
            socket.emit("created", {
                roomID: roomID
            })
            socket.join(roomID);
            inRoom.push({
                roomID: roomID,
                isEmpty: true,
                hostID: socket.id
            })
            console.log(data.id + " was created a room. room id: " + roomID);
        } else {
            for (var key in inRoom) {
                if (inRoom[key].isEmpty && inRoom[key].hostID != socket.id) {
                    socket.emit("created", {
                        roomID: roomID
                    })
                    socket.join(inRoom[key].roomID);
                    hasFound = true;
                    inRoom[key].isEmpty = false;
                    inRoom[key].clientID = socket.id;
                    console.log(data.id + " was found a room. room id: " + roomID);

                    socket.emit('join', "hello");
                    socket.in(inRoom[key].roomID).emit('join', "hello");
                    break;
                } else if (inRoom[key].hostID == socket.id) {
                    created = true
                    break;
                }
            }
            if (!hasFound && created == false) {
                roomID++;
                socket.emit("created", {
                    roomID: roomID
                })

                socket.join(roomID);

                inRoom.push({
                    roomID: roomID,
                    isEmpty: true,
                    hostID: data.id
                })
                console.log(data.id + " was created a room. room id: " + roomID);
            }
        }
    });

    socket.on('leave', function (data) {
        io.sockets.in(data.roomID).emit("leave", "byebye");
        socket.leaveAll();
        for (const key in inRoom) {
            if (inRoom[key].hostID == data.id) {
                inRoom.pop(key);
                console.log(data.id + " was leaved a room. room id: " + roomID)
                break;
            }
        }
    })

    socket.on('disconnect', function (reason) {
        console.log(reason);
        socket.leaveAll();
        for (const key in inRoom) {
            if (inRoom[key].hostID == socket.id || inRoom[key].clientID == socket.id) {
                socket.in(inRoom[key].roomID).emit("leave", "bye");
                inRoom.pop(key);
                console.log(socket.id + " was leaved a room. room id: " + roomID)
                break;
            }
        }
    });
})
console.log('server start on localhost:' + port);