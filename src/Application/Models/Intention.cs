using System.Security.Cryptography;

namespace Application.Models;

public class Intention
{
    public Intention(string user)
    {
        User = user;
        IntentionId = Guid.NewGuid();
        Keyboard = GenerateKeyboard();
        ExpireIn = DateTimeOffset.Now.AddMinutes(2);
    }

    public Guid IntentionId { get; set; }
    public string User { get; set; }
    public IEnumerable<Keyboard> Keyboard { get; set; }
    public DateTimeOffset ExpireIn { get; set; }


    private static IEnumerable<Keyboard> GenerateKeyboard()
    {
        var collection = new List<Keyboard>();

        var options = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

        while (options.Count > 0)
        {
            var keyboard = new Keyboard
            {
                Id = Guid.NewGuid(),
                Values = new List<int>()
            };

            AddNumber(options, keyboard);
            AddNumber(options, keyboard);

            collection.Add(keyboard);
        }

        return collection;
    }

    private static void AddNumber(IList<int> options, Keyboard keyboard)
    {
        var i = RandomNumberGenerator.GetInt32(options.Count);
        keyboard.Values.Add(options[i]);
        options.Remove(options[i]);
    }

    public IEnumerable<string>? GetPossiblePasswords(IEnumerable<Guid> password)
    {
        var keys = new int[password.Count()][];

        for (var i = 0; i < keys.Length; i++)
        {
            var p = password.ElementAt(i);
            var k = Keyboard.FirstOrDefault(f => f.Id == p);

            if (k is null)
                return null;

            var key = k.Values;

            keys[i] = key.ToArray();
        }

        return GetPossiblePasswords(keys, 0);
    }

    private static IEnumerable<string> GetPossiblePasswords(IReadOnlyList<int[]> keys, int i)
    {
        var l = new List<string>();
        var ks = keys[i];

        if (i == keys.Count - 1)
        {
            l.Add(ks[0].ToString());
            l.Add(ks[1].ToString());

            return l;
        }

        foreach (var k in ks)
        {
            var p = GetPossiblePasswords(keys, i + 1);
            l.AddRange(p.Select(s => k + s));
        }

        return l;
    }
}