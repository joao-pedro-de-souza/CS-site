

namespace post
{
    public class Post
    {
        public int ID{ get; set; }
        public  string Message;
        public Post(string input){            
            string trimmedInput = input.Trim('(', ')');
            string[] parts = trimmedInput.Split(',');

            if (parts.Length == 2)
            {
            string part1 = parts[0].Trim(' ', '"');
                if (int.TryParse(parts[1].Trim(), out int part2))
                {
                var myTuple = (part1, part2);
                ID = myTuple.part2;
                Message=myTuple.part1;
                }  
            }
        }

    }
}
