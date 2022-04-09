
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData))]
[CanEditMultipleObjects]
public class EditorLevel : Editor
{
	private const string STR_LEVEL = "SET_LEVEL";
	private const string STR_LEVEL_DES = "����";
	private const string STR_RESET_DES = "�ʱ�ȭ";
	private const string STR_RESTART_DES = "���Ӵٽý���";

	private const string STR_LINE_OPEN = "���μ��� ����Ʈ ����";
	private const string STR_LINE_CLOSE = "���μ��� ����Ʈ ����";

	private const string STR_PRINT = "���";

	private const string STR_DESC01 = "No : ";
	private const string STR_DESC02 = "����";
	private const string STR_DESC03 = "�������";
	private const string STR_DESC04 = "üũ�ܰ�";

	private const string STR_DESC05 = "ȭ��ǥ�ýð�";

	private const string STR_DESC06 = "�̵� �ӵ�";
	private const string STR_DESC07 = "�¿� �̵� �ּҰ�";
	private const string STR_DESC08 = "�¿� �̵� �ִ밪";
	private const string STR_DESC09 = "���Ʒ� �̵� �ּ� ��";
	private const string STR_DESC10 = "���Ʒ� �̵� �ִ� ��";
	private const string STR_DESC11 = "���� ���� �ּҰ�";
	private const string STR_DESC12 = "���� ���� �ִ밪";

	private const string STR_DESC13 = "�ٶ� �ӵ�";

	private const string STR_RELOAD = "������ �ҷ�����";

	private const string STR_MOVE_Y = "StartY�� �̵�";

	//private bool isLineOpen = false;

	private string str_result = string.Empty;

	public override void OnInspectorGUI()
	{
		if (GUILayout.Button(STR_RELOAD))
		{
			if (LevelData.ins != null) LevelData.ins.LoadData(true);
		}
		if (GUILayout.Button(STR_RESTART_DES))
		{
			if (PlayManager.ins != null) PlayManager.ins.GameOver();

		}
		/*
		if (GUILayout.Button(STR_MOVE_Y))
		{
			if (PlayManager.ins != null) PlayManager.ins.RestartY();
		}
		*/
		base.OnInspectorGUI();
		GUILayout.Space(10);
		

		
		GUILayout.Space(10);



		//GUILayout.BeginHorizontal();
		/*
		GUILayout.Toggle(PlayerPrefs.HasKey(STR_LEVEL), string.Empty, GUILayout.Width(20));
		{

		}

		if (GUILayout.Button(STR_LEVEL_DES))
		{

		}
		*/

		/*
		LevelData data = (LevelData)target;

		if (isLineOpen)
		{
			if (GUILayout.Button(STR_LINE_CLOSE)) {	isLineOpen = false; }
			
			if (data != null && data.lines != null)
			{
				for (int i = 0; i < data.lines.Count; i++)
				{
					GUILayout.Label(STR_DESC01 + i);

					GUILayout.BeginHorizontal();
					GUILayout.Label(STR_DESC02);
					GUILayout.TextField(data.lines[i].order.ToString());
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					GUILayout.Label(STR_DESC03);
					data.lines[i].startY = int.Parse(GUILayout.TextField(data.lines[i].startY.ToString()));
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					GUILayout.Label(STR_DESC04);
					data.lines[i].checkGap = int.Parse(GUILayout.TextField(data.lines[i].checkGap.ToString()));
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					GUILayout.Label(STR_DESC05);
					data.lines[i].checkGap = int.Parse(GUILayout.TextField(data.lines[i].time.ToString()));
					GUILayout.EndHorizontal();



				}

			}
		}
		else
		{
			if (GUILayout.Button(STR_LINE_OPEN)) {	isLineOpen = true; }
		}

		GUILayout.Space(10);
		if (GUILayout.Button(STR_RESET_DES))
		{
			data.Awake();
		}

		GUILayout.Space(10);
		if (GUILayout.Button(STR_PRINT))
		{
			str_result = "		" + "playY = " + data.playY + ";\n"
			+ "		" + "gravity = " + data.gravity + "f;\n"
			+ "\n"
			+ "		" + "lineGap = " + data.lineGap + ";\n"
			+ "		" + "lineWidth = " + data.lineWidth + ";\n"
			+ "		" + "lineHeight = " + data.lineHeight + "f;\n"
			+ "\n"
			+ "		" + "lineForceRate = " + data.lineForceRate + "f;\n"
			+ "		" + "lineDragMax = " + data.lineDragMax + ";\n"
			+ "		" + "lineForce = " + data.lineForce + "f;\n"
			+ "\n"
			+ "		" + "guideLength = " + data.guideLength + ";\n"
			+ "		" + "guideDetail = " + data.guideDetail + ";\n"
			+ "\n"
			;

			if (data.lines != null)
			{
				str_result += 
				"		" + "lines = new List<LineData>()" + ";\n"
				+ "\n";

				for (int i = 0; i < data.lines.Count; i++)
				{
					str_result +=
						"		" + "lines.Add(new LineData())" + ";\n"
						+ "		" + "lines[lines.Count - 1].order = " + data.lines[i].order + ";\n"
						+ "		" + "lines[lines.Count - 1].startY = " + data.lines[i].startY + ";\n"
						+ "		" + "lines[lines.Count - 1].checkGap = " + data.lines[i].checkGap + ";\n"
						+ "		" + "lines[lines.Count - 1].time = " + data.lines[i].time + ";\n"
						+ "		" + "lines[lines.Count - 1].width_max = " + data.lines[i].width_max + ";\n"
						+ "		" + "lines[lines.Count - 1].width_min = " + data.lines[i].width_min + ";\n"
						+ "		" + "lines[lines.Count - 1].move_speed = " + data.lines[i].move_speed + ";\n"
						+ "		" + "lines[lines.Count - 1].move_X_max = " + data.lines[i].move_X_max + ";\n"
						+ "		" + "lines[lines.Count - 1].move_X_min = " + data.lines[i].move_X_min + ";\n"
						+ "		" + "lines[lines.Count - 1].move_y_max = " + data.lines[i].move_y_max + ";\n"
						+ "		" + "lines[lines.Count - 1].move_y_min = " + data.lines[i].move_y_min + ";\n"
						+ "		" + "lines[lines.Count - 1].windSpeed = " + data.lines[i].windSpeed + ";\n"
						+ "\n"
						;
				}
			}
			
		}

		GUILayout.TextArea(str_result, GUILayout.Height(500));
		*/
	}
}

