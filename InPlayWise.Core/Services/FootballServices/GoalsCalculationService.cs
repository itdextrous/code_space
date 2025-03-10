using InPlayWise.Common.Constants;
using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Core.Services.FootballServices
{
    public static class GoalsCalculationService
    {
        public static List<int> GoalsFirstHalf(string goalMinutes)
        {
            try
            {
                List<int> res = new List<int>();
                if (goalMinutes.Equals(""))
                {
                    return res;
                }
                foreach (var goalMinute in goalMinutes.Split(","))
                {
                    if(goalMinute.Equals("")) {
                        return new List<int>();
                    }
                    if (goalMinute.Contains("+"))
                    {
                        string timeStr = goalMinute.Substring(0, goalMinute.IndexOf("+"));
                        int time = int.Parse(timeStr);
                        if (time == 90)
                            continue;
                        string extra = goalMinute.Substring(goalMinute.IndexOf("+") + 1);
                        int extraTime = int.Parse(extra);
                        res.Add((int)(45 + extraTime));
                    }
                    else
                    {
                        if (int.Parse(goalMinute) > 45)
                            continue;
                        res.Add(int.Parse(goalMinute));
                    }
                }
                return res;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new List<int>();
            }
        }

        public static List<int> GoalsSecondHalf(string goalMinutes)
        {
            try
            {
                List<int> res = new List<int>();
                if (goalMinutes.Equals("")) return res;
                
                foreach (var goalMinute in goalMinutes.Split(","))
                {
                    if (goalMinute.Contains("+"))
                    {
                        string timeStr = goalMinute.Substring(0, goalMinute.IndexOf("+"));
                        int time = int.Parse(timeStr);
                        if (time == 45)
                            continue;
                        string extra = goalMinute.Substring(goalMinute.IndexOf("+") + 1);
                        int extraTime = int.Parse(extra);
                        res.Add((int)(90 + extraTime));
                    }
                    else
                    {
                        if (int.Parse(goalMinute) <= 45)
                            continue;
                        res.Add(int.Parse(goalMinute));
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<int>();
            }
        }



        public static List<int> GoalsOvertime(string goalMinutes)
        {
            try
            {
                List<int> res = new List<int>();
                if (goalMinutes.Equals("")) return res;

                foreach (var goalMinute in goalMinutes.Split(","))
                {
                    if (goalMinute.Contains("+"))
                    {
                        string timeStr = goalMinute.Substring(0, goalMinute.IndexOf("+"));
                        int time = int.Parse(timeStr);
                        if (time != 105)
                            continue;
                        string extra = goalMinute.Substring(goalMinute.IndexOf("+") + 1);
                        int extraTime = int.Parse(extra);
                        res.Add((int)(105 + extraTime));
                    }
                    else
                    {
                        if (int.Parse(goalMinute) <= 90)
                            continue;
                        res.Add(int.Parse(goalMinute));
                    }
                }
                return res;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new List<int>();
            }
        }



        public static bool HomeScoredFirstGoal(string homeGoalMinutes, string awayGoalMinutes)
        {
            try
            {
                if (string.IsNullOrEmpty(homeGoalMinutes) && string.IsNullOrEmpty(awayGoalMinutes))
                    return false;

                List<int> homeGoalsFirstHalf = GoalsFirstHalf(homeGoalMinutes);
                List<int> awayGoalsFirstHalf = GoalsFirstHalf(awayGoalMinutes);

                if (homeGoalsFirstHalf.Count == 0 && awayGoalsFirstHalf.Count != 0) return false;
                if (homeGoalsFirstHalf.Count != 0 && awayGoalsFirstHalf.Count == 0) return true;
                if(!(homeGoalsFirstHalf.Count == 0 && awayGoalsFirstHalf.Count == 0))
                    if (homeGoalsFirstHalf[0] < awayGoalsFirstHalf[0]) return true;



                List<int> homeGoalsSecondHalf = GoalsSecondHalf(homeGoalMinutes);
                List<int> awayGoalsSecondHalf = GoalsSecondHalf(awayGoalMinutes);

                if (homeGoalsSecondHalf.Count == 0 && awayGoalsSecondHalf.Count != 0) return false;
                if (homeGoalsSecondHalf.Count != 0 && awayGoalsSecondHalf.Count == 0) return true;
                if (!(homeGoalsSecondHalf.Count == 0 && awayGoalsSecondHalf.Count == 0))
                    if (homeGoalsSecondHalf[0] < awayGoalsSecondHalf[0]) return true;



                List<int> homeGoalsOvertime = GoalsOvertime(homeGoalMinutes);
                List<int> awayGoalsOvertime = GoalsOvertime(awayGoalMinutes);

                if (homeGoalsOvertime.Count == 0 && awayGoalsOvertime.Count != 0) return false;
                if (homeGoalsOvertime.Count != 0 && awayGoalsOvertime.Count == 0) return true;
                if (!(homeGoalsOvertime.Count == 0 && awayGoalsOvertime.Count == 0))
                    if (homeGoalsOvertime[0] < awayGoalsOvertime[0]) return true;

                return false;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public static List<int> GetAllGoals(string goalMinutes)
        {
            try
            {
                List<int> result = new List<int>();
                List<int> firstHalf = GoalsFirstHalf(goalMinutes);
                List<int> secondHalf = GoalsSecondHalf(goalMinutes);
                List<int> overTime = GoalsOvertime(goalMinutes);
                result.AddRange(firstHalf);
                result.AddRange(secondHalf);
                result.AddRange(overTime);
                return result;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new List<int>();
            }
        }


        public static int GetFirstGoalMinute(string goalMinutes)
        {
            try
            {
                List<int> goalsFirstHalf = GoalsFirstHalf(goalMinutes);
                if (goalsFirstHalf.Count == 0) return -1;
                return goalsFirstHalf[0];
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }


        public static int GetNumOfGoalsInInterval(int time1, int time2, string goalMinutes)
        {
            try
            {
                List<int> goalsFirstHalf = GetAllGoals(goalMinutes);
                int count = 0;
                foreach(int goal in goalsFirstHalf)
                {
                    if (goal >= time1 && goal <= time2) count++;
                }
                return count;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }


        public static int GetNumOfGoalsInIntervalInSecondHalf(int time1, int time2, string goalMinutes)
        {
            try
            {
                List<int> goalsFirstHalf = GoalsSecondHalf(goalMinutes);
                int count = 0;
                foreach (int goal in goalsFirstHalf)
                {
                    if (goal >= time1 && goal <= time2) count++;
                }
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }






        public static List<List<int>> GetGoalMinutes(string goalMinutes, List<int> halves)
        {
            try
            {

				//List<int> goalsInt = goalMinutes.Split(',').Select(int.Parse).ToList();
				//List<List<int>> res = new List<List<int>>();

				//for (int z = 0; z < 3; z++)
				//	res.Add(new List<int>());

				// Check if the number of goals is at least as long as the third half
				//if (goalsInt.Count < halves.Sum())
				//{
				//	return res;
				//}

				//int startIndex = 0;
				//for (int i = 0; i < halves.Count; i++)
				//{
				//	for (int j = 0; j < halves[i]; j++)
				//	{
				//		res[i].Add(goalsInt[startIndex]);
				//		startIndex++;
				//	}
				//}

				//return res;

				List<List<int>> res = new List<List<int>>();

				for (int z = 0; z < 3; z++) res.Add(new List<int>());


				if (string.IsNullOrWhiteSpace(goalMinutes))
                    return res;

                List<int> goalsInt = goalMinutes.Split(',').Select(int.Parse).ToList();


                if (goalsInt.Count < halves[2])
                {
                    return res;
                }

                for (int i = 0; i < halves[0]; i++)
                {
                    res[0].Add(goalsInt[i]);
                }

                for (int j = halves[0]; j < halves[1]; j++)
                {
                    res[1].Add(goalsInt[j]);
                }

                if (halves[2] == 0)
                    return res;

                for (int k = halves[1]; k < halves[2]; k++)
                {
                    res[2].Add(goalsInt[k]);

                }
                return res;
            }
            catch(Exception ex)
            {
                Console.WriteLine("goals minutes is" + goalMinutes);
                Console.WriteLine(ex.ToString());
                return new List<List<int>> { new List<int>() };
            }
        }


        public static int GetFirstGoalMinute(string goalMinutes, List<int> halves)
        {
            try
            {
                List<List<int>> mins = GetGoalMinutes(goalMinutes, halves);
                foreach(var x in mins)
                {
                    foreach(var y in x)
                    {
                        return y;
                    }
                }
                return -1;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }


        public static int GetLastGoalMinute(string goalMinutes, List<int> halves)
        {
            try
            {
                List<List<int>> mins = GetGoalMinutes(goalMinutes, halves);
                int last = -1;
                foreach (var x in mins)
                {
                    foreach (var y in x)
                    {
                        last = y;
                    }
                }
                return last;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }


        public static int GetGoalsScoredInInterval(string goalMinutes, List<int> halves, int start, int end)
        {
            try
            {
				List<List<int>> mins = GetGoalMinutes(goalMinutes, halves);
                int count = 0;
				foreach (var x in mins)
				{
					foreach (var y in x)
					{
                        if (y >= start && y <= end)
                            count++;
					}
				}
                return count;
			}
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }


        public static MatchResult GetMatchResult(RecentMatchDto match)
        {
            try
            {
                List<int> homeGoals = new List<int>();
                List<int> awayGoals = new List<int>();
                match.HomeGoalMinutes.Split(',').ToList().ForEach(mn => homeGoals.Add(int.Parse(mn)));
				match.AwayGoalMinutes.Split(',').ToList().ForEach(mn => awayGoals.Add(int.Parse(mn)));

				if (match.GameDrawn)
				{
                    if(homeGoals.Count == 0 && awayGoals.Count == 0)
					    return new MatchResult() { Draw = true };
                    if (homeGoals[^1] < awayGoals[^1])
                        return new MatchResult() { Draw = true, AwayTeamComeBackToDraw = true };
                    return new MatchResult() { Draw = true, HomeTeamComeBackToDraw = true };
				}

				if (homeGoals.Count == 0 && awayGoals.Count == 0)
                {
                    if (match.HomeWin)
                        return new MatchResult() { HomeTeamWin = true };
                    else
                        return new MatchResult() { AwayTeamWin = true };
                }
                if (match.HomeWin)
                {
                    if (awayGoals.Count == 0)
                        return new MatchResult() { HomeTeamWin = true };
                    if (homeGoals[0] > awayGoals[0])
                        return new MatchResult() { HomeTeamWin = true, ComeBackToWin = true };
                    return new MatchResult() { HomeTeamWin = true };
                }
                else
                {
					if (homeGoals.Count == 0)
						return new MatchResult() { AwayTeamWin = true };
					if (awayGoals[0] > homeGoals[0])
						return new MatchResult() { AwayTeamWin = true, ComeBackToWin = true };
					return new MatchResult() { AwayTeamWin = true };
				}
			}
            catch(Exception ex)
            {
                if (match.GameDrawn) return new MatchResult() { Draw = true };
                if (match.HomeWin) return new MatchResult() { HomeTeamWin = true };
                return new MatchResult() { AwayTeamWin = true };
            }
        }

    }
}
