const express=require("express");
const app=express();
const Room = require('./room')
const{nanoid}=require("nanoid")
bodyParser=require('body-parser')

app.use(bodyParser.json())
app.use(bodyParser.urlencoded({extended:false}))


let Rooms = new Array()

app.post("/startRoom",function(req,res){
    if(Rooms.length > 0)
    {
        let iJoined = false
        Rooms.forEach(room => {
            if(room.playerCount < 2)
            {
                let newPlayer = req.body;
                newPlayer.RoomID = room.roomId;
                room.playerCount++;
                room.players.push(newPlayer);
                    
                //console.log(Rooms);
                //console.log(room.playerCount, " ", room.players);
            
                res.send(newPlayer); 
                iJoined = true;
                console.log (`player ${newPlayer.playerName} joined the room ${room.roomId}`)
            }
        });

        if(iJoined == false)
        {
            createRoom(req, res);
        }

    }
    else{
        createRoom(req, res);
    }

})

app.get('/fetchOtherPlayerData/:roomId/:myName', (req, res)=>{

    let myRoomId = req.params.roomId;
    let myName = req.params.myName;
    let myRoom = "";
    let gotOtherPlayerData = false;
    Rooms.forEach(room => {
        if(room.roomId == myRoomId)
        {
            myRoom = room;

        }
    });
    myRoom.players.forEach(player => {
        //console.log(player.playerName);
        if(player.playerName != myName)
        {
            gotOtherPlayerData = true;
            res.send(player);
        }
    });

   // console.log("my room id ", myRoomId, " ", myName);
    if(gotOtherPlayerData === false) res.send({"otherPlayer": false});
})

app.get('/fetchscore/:roomId/:myName/:myScore', (req, res)=>{

    let myRoomId = req.params.roomId;
    let myName = req.params.myName;
    let myScore = req.params.myScore;
    let myRoom = "";
    let gotOtherPlayerData = false;
    Rooms.forEach(room => {
        if(room.roomId == myRoomId)
        {
            myRoom = room;

        }
    });
    myRoom.players.forEach(player => {
       // console.log(player.playerName);
        if(player.playerName != myName)
        {
            gotOtherPlayerData = true;
            res.send(String(player.score));
        }
        else{
            player.score=myScore;
        }
    });

    //console.log("my room id ", myRoomId, " ", myName);
    if(gotOtherPlayerData === false) res.send({"otherPlayer": false});
})

app.get('/getWinner/:roomId/:myName/:myScore/:finishedPlaying/:foundWinner', (req, res)=>{

    let myRoomId = req.params.roomId;
    let myName = req.params.myName;
    let myScore = req.params.myScore;
    let finishedPlaying = req.params.finishedPlaying;
    let foundWinner = req.params.foundWinner;
    let myRoom = "";
    let otherPlayer = '';
    let myPlayer = '';

    if (foundWinner == true) return;
    //console.log(myRoomId,myName,myScore,finishedPlaying)
    Rooms.forEach(room => {
        if(room.roomId == myRoomId)
        {
            myRoom = room;

        }
    });
    myRoom.players.forEach(player => {
       // console.log(player.playerName);
        if(player.playerName != myName)
        {
            // res.send(player.score);
            otherPlayer = player;
        }
        else{
            player.finishedPlaying = true;
            player.score=myScore;
            myPlayer = player;
            
        }
    });

   // console.log("my room id ", myRoomId, " ", myName);
    findWinner(finishedPlaying, otherPlayer, myPlayer.score, res, myPlayer);
})



app.listen(3000,function(req,res){console.log("server is running at port no.3000")})

function findWinner(finishedPlaying, otherPlayer, myScore, res, myPlayer) {
    console.log("Trying to find winner", finishedPlaying , " ", otherPlayer.finishedPlaying);
    if (myPlayer.finishedPlaying == true && otherPlayer.finishedPlaying == true) {
        console.log("declaring Winner")
        if ( parseInt(myScore) > parseInt(otherPlayer.score)) {
            console.log(myScore, " is the winning score and losing score is", otherPlayer.score);
            myPlayer.iWon = true;
            res.send(myPlayer);
        }
        else if ( parseInt(otherPlayer.score) > parseInt(myScore)) {
            console.log(otherPlayer.score, " is the winning score and the losing score is", myScore);
            otherPlayer.iWon = true;
            res.send(otherPlayer);
            //other guy won
        }
        else if (otherPlayer.score == myScore) {
            //match drawn
        }
        

    }
    else {
        res.send("false");
    }
}

function createRoom(req, res) {
    let newRoom = new Room(nanoid(20), 0);
    let newPlayer = req.body;
    newPlayer.RoomID = newRoom.roomId;
    newRoom.playerCount++;
    newRoom.players.push(newPlayer);

    Rooms.push(newRoom);

   // console.log(Rooms);
   // console.log(Rooms[0].players);
   console.log(newRoom.roomId, " room Created for new Player with id ",newPlayer.playerName)

    res.send(newPlayer);
}
