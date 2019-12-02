const mongoose = require('mongoose');

mongoose.Promise = global.Promise;

var schema = new mongoose.Schema({
    username:{
        type:String,
        required: true        
    },
    email:{
        type:String,
        required:false
    },
    date_login:{
        type:Number,
        required:true
    }
})

module.exports = mongoose.model("history",schema,"history_login");