public class CharacterInGame:Character
{
    public CharacterInGame(Character character):base(Utils.Clone<Character>(character))
    {
        
    }

    public CharacterInGame()
    {
    }
}
