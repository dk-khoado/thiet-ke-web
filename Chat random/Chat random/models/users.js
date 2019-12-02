const mongoose = require('mongoose');

mongoose.Promise = global.Promise;
// Declare the Schema of the Mongo model
var userSchema = new mongoose.Schema({
    name: {
        type: String,
        minlength:1         
    },
    username:{
        type:String,
        required:true,
        unique:true
    },
    gender:{
        type:String,
        default:"Nam"
    },
    email: {
        type: String,
        required: true,
        unique: true      
    },
    password: {
        type: String,
        required: true
    }
});

//Export the model
module.exports = mongoose.model('User', userSchema);