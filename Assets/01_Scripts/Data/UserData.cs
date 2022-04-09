using System.Collections.Generic;
class UserData
{
	public int id;
	public int id_main_hero;
	public int level;
	public ulong exp;
	public string nickname;
	public int frame;
	public int icon;
	public string country;

	public CurrencyData currency;

	public List<int> list_hero;
	public List<int> list_item;
	public List<int> list_equip;

}

class CurrencyData 
{
	public ulong diamond;
	public ulong gold;
	public ulong stamina;
}

class CharacterData 
{
	public int id;
	public List<int> list_message;
	public int hp;
	public int mp;
	public int power;
	public List<int> list_attack;
}

class HeroData : CharacterData
{

}

class MonsterData : CharacterData 
{
	public int grow;
	public int grade;
}


class ModeData 
{
	public int id;
	public List<int> list_stage;
	public int start_coin;
	public int start_humancount;
	public int max_humancount;
	public int cost_humancount;
	public bool is_refresh;
	public int cost_resfresh;
	public int time_ready;
}

class StageData 
{
	public int id;
	public int id_res_background;
	public List<int> list_wave;
}

class WaveData 
{
	public int id;
	public List<int> list_monster;
}


class ItemData
{
	public int id;
	public int id_res_icon;
}

class BattleItemData : ItemData 
{
	public int id_fx;
}

class EquipItemData : ItemData 
{
	public int power;
	public int defence;
}

class DropItemData : ItemData 
{
	public int rate;
}

class ResourceData 
{
	public int id;
	public string location;
}

class SoundData
{
	public int id;
	public string location;
}

class CountryData 
{
	int id;
	string key;
}

class MessageData_KR 
{
	int id;
	string message;
	int id_sound;
}

class MessageData_JP
{
	int id;
	string message;
	int id_sound;
}