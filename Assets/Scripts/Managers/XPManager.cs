using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class XPManager
{
    public static int CalculateXP(Enemy e)
    {
        //  每级经验值 = (Char Level * 5) +45, 当角色等级=怪物等级, 为了艾泽拉斯

        int baseXP = (Player.Instance.Level * 5) + 45;
        // 计算等级阈值
        int grayLevel = CalculateGrayLevel();

        int totalXP = 0;

        //XP = (Base XP) *(1 + 0.05 * (Mob Level - Char Level) ), 当怪物等级大于角色等级

        if (e.Level >= Player.Instance.Level)
        {
            totalXP = (int)(baseXP * (1 + 0.05 * (e.Level - Player.Instance.Level)));
        }
        else if (e.Level > grayLevel)
        {
            totalXP = (baseXP) * (1 - (Player.Instance.Level - e.Level) / ZeroDifference());
        }

        return totalXP;
    }

    public static int CalculateXP(Quest e)
    {
        if (Player.Instance.Level <= e.Level +5)
        {
            return e.Xp;
        }
        if (Player.Instance.Level == e.Level + 6)
        {
           return (int)(e.Xp * 0.8/5)*5;
        }
        if (Player.Instance.Level == e.Level + 7)
        {
            return (int)(e.Xp * 0.6 / 5) * 5;
        }
        if (Player.Instance.Level == e.Level + 8)
        {
            return (int)(e.Xp * 0.4 / 5) * 5;
        }
        if (Player.Instance.Level == e.Level + 9)
        {
            return (int)(e.Xp * 0.2 / 5) * 5;
        }
        if (Player.Instance.Level >= e.Level + 10)
        {
            return (int)(e.Xp * 0.1 / 5) * 5;
        }

        return 0;
    }

    private static int ZeroDifference()
    {
        if (Player.Instance.Level <= 7)
        {
            return 5;
        }
        if (Player.Instance.Level >= 8 && Player.Instance.Level <= 9)
        {
            return 6;
        }
        if (Player.Instance.Level >= 10 && Player.Instance.Level <= 11)
        {
            return 7;
        }
        if (Player.Instance.Level >= 12 && Player.Instance.Level <= 15)
        {
            return 8;
        }
        if (Player.Instance.Level >= 16 && Player.Instance.Level <= 19)
        {
            return 9;
        }
        if (Player.Instance.Level >= 20 && Player.Instance.Level <= 29)
        {
            return 11;
        }
        if (Player.Instance.Level >= 30 && Player.Instance.Level <= 39)
        {
            return 12;
        }
        if (Player.Instance.Level >= 40 && Player.Instance.Level <= 44)
        {
            return 13;
        }
        if (Player.Instance.Level >= 45 && Player.Instance.Level <= 49)
        {
            return 14;
        }
        if (Player.Instance.Level >= 50 && Player.Instance.Level <= 54)
        {
            return 15;
        }
        if (Player.Instance.Level >= 55 && Player.Instance.Level <= 59)
        {
            return 16;
        }

        return 17;

    }

    public static int CalculateGrayLevel()
    {
        if (Player.Instance.Level <= 5)
        {
            return 0;
        }
        else if (Player.Instance.Level >= 6 && Player.Instance.Level <= 49)
        {
            return Player.Instance.Level - (Player.Instance.Level / 10) - 5; 
        }
        else if (Player.Instance.Level == 50)
        {
            return Player.Instance.Level - 10;
        }
        else if (Player.Instance.Level >= 51 && Player.Instance.Level <= 59)
        {
            return Player.Instance.Level - (Player.Instance.Level / 5) - 1;
        }

        return Player.Instance.Level - 9;
    }
}