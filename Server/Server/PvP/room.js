class Room {
    constructor(roomId, winnerScore){
        this.roomId = roomId;
        this.players = [];
        this.playerCount = 0;
        this.winnerScore = winnerScore;
    }
}

module.exports = Room;