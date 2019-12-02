var express = require('express')
var route = express.Router();


route.get('/', function(req,res){
    res.render("../Views/index.ejs",{body:"login"});
});

route.get('/signup', function(req,res){    
    res.render("../Views/index.ejs",{body:"signup"});
})
route.get('/main', function(req,res){
    res.render("../Views/index.ejs",{body:"chat_room"});
})
module.exports = route;