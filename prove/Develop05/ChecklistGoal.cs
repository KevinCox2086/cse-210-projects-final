public class ChecklistGoal : Goal
{
    private int _amountCompleted;
    private int _target;
    private int _bonusPoints;

    public ChecklistGoal(string name, string description, int points, int target, int bonusPoints) : base(name, description, points)
    {
        _target = target;
        _bonusPoints = bonusPoints;
        _amountCompleted = 0;
    }

    public ChecklistGoal(string name, string description, int points, int target, int bonusPoints, int amountCompleted) : base(name, description, points)
    {
        _target = target;
        _bonusPoints = bonusPoints;
        _amountCompleted = amountCompleted;
    }

    public override int RecordEvent()
    {
        if (!IsComplete())
        {
            _amountCompleted++;
            Console.WriteLine($"Congratulations! You have recorded an event for the goal: {_shortName}");
            
            if (IsComplete())
            {
                Console.WriteLine("You have completed this goal! A bonus is awarded!");
                Console.WriteLine($"You have earned {_points} + {_bonusPoints} (bonus) points!");
                return _points + _bonusPoints;
            }
            else
            {
                Console.WriteLine($"You have earned {_points} points!");
                Console.WriteLine($"You are now {(_amountCompleted / (double)_target) * 100:F0}% of the way there!");
                return _points;
            }
        }
        else
        {
            Console.WriteLine($"You have already completed the goal: {_shortName}");
            return 0;
        }
    }

    public override bool IsComplete()
    {
        return _amountCompleted >= _target;
    }

    public override string GetDetailsString()
    {
        string status = IsComplete() ? "[X]" : "[ ]";
        return $"{status} {_shortName} ({_description}) -- Currently completed: {_amountCompleted}/{_target}";
    }

    public override string GetStringRepresentation()
    {
        return $"ChecklistGoal:{_shortName}:{_description}:{_points}:{_bonusPoints}:{_target}:{_amountCompleted}";
    }
}