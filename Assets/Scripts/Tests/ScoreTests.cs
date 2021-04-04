using AirHockey.Match;
using NUnit.Framework;

namespace AirHockey.Test
{
    public class ScoreTests
    {
        [Test]
        public void StartIsTie()
        {
            var score = new Score();
            Assert.AreEqual(score.FinalResult, Score.Result.Tie, "Initial score is a tie.");
        }
        
        [Test]
        public void OneEachIsTie()
        {
            var score = new Score();
            score.ScoreGoal(Player.LeftPlayer);
            score.ScoreGoal(Player.RightPlayer);
            Assert.AreEqual(score.FinalResult, Score.Result.Tie, "One goal each is a tie.");
        }
        
        [Test]
        public void ManyEachIsTie()
        {
            var score = new Score();
            
            for (var i = 0; i<100; i++)
                score.ScoreGoal(Player.LeftPlayer);
            
            for (var i = 0; i<100; i++)
                score.ScoreGoal(Player.RightPlayer);
            
            Assert.AreEqual(score.FinalResult, Score.Result.Tie, "Many goals each is a tie.");
        }

        [Test]
        public void FullMatch()
        {
            var score = new Score();
            
            score.ScoreGoal(Player.LeftPlayer);
            Assert.AreEqual(score.FinalResult, Score.Result.LeftPlayerWin, "1-0: left wins.");
            
            score.ScoreGoal(Player.RightPlayer);
            Assert.AreEqual(score.FinalResult, Score.Result.Tie, "1-1: tie.");
            
            score.ScoreGoal(Player.RightPlayer);
            Assert.AreEqual(score.FinalResult, Score.Result.RightPlayerWin, "1-2: right wins.");
            
            score.ScoreGoal(Player.LeftPlayer);
            Assert.AreEqual(score.FinalResult, Score.Result.Tie, "2-2: tie.");
            
            for (var i = 0; i<100; i++)
                score.ScoreGoal(Player.LeftPlayer);
            
            score.ScoreGoal(Player.LeftPlayer);
            Assert.AreEqual(score.FinalResult, Score.Result.LeftPlayerWin, "102-2: left wins.");
            
            for (var i = 0; i<100; i++)
                score.ScoreGoal(Player.RightPlayer);
            
            score.ScoreGoal(Player.RightPlayer);
            Assert.AreEqual(score.FinalResult, Score.Result.Tie, "102-102: tie.");
        }
        
    }
}