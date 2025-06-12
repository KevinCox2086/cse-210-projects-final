// Represents the Breathing Activity, guiding the user through paced breathing.
public class BreathingActivity : Activity
{
    // Initializes a new instance of the BreathingActivity class.
    public BreathingActivity() : base("Breathing Activity", "This activity will help you relax by walking your through breathing in and out slowly. Clear your mind and focus on your breathing.")
    {
    }

    // Runs the core logic for the breathing activity, alternating between "Breathe in" and "Breathe out".
    public override void RunActivity()
    {
        DateTime endTime = DateTime.Now.AddSeconds(_duration);

        while (DateTime.Now < endTime)
        {
            AnimateBreathingBar(4, "Breathe in... ");
            Console.WriteLine();

            // Ensure we don't start the next phase if time is already up
            if (DateTime.Now >= endTime) break;

            AnimateBreathingBar(6, "Breathe out... ");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
    // This is also another part of the showing creativity
    // Displays a smooth, animated progress bar by timing each character change.
    private void AnimateBreathingBar(int seconds, string message)
    {
        const int barWidth = 30; // A wider bar for a better visual effect
        int timePerBlock = (seconds * 1000) / barWidth; // Calculate delay for each block

        if (message.Contains("in"))
        {
            // Inhaling: fill the bar from left to right
            for (int i = 0; i <= barWidth; i++)
            {
                string bar = new string('█', i);
                string empty = new string('-', barWidth - i);
                Console.Write($"\r{message} [{bar}{empty}]");
                Thread.Sleep(timePerBlock);
            }
        }
        else // "out"
        {
            // Exhaling: empty the bar from right to left
            for (int i = barWidth; i >= 0; i--)
            {
                string bar = new string('█', i);
                string empty = new string('-', barWidth - i);
                Console.Write($"\r{message} [{bar}{empty}]");
                Thread.Sleep(timePerBlock);
            }
        }
    }
}