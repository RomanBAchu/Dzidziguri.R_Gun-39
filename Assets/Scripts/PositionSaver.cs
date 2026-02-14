using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Unity.Collections;
using UnityEngine;

namespace DefaultNamespace
{
	public class PositionSaver : MonoBehaviour
	{
		[Serializable]

		public struct Data
		{
			public Vector3 Position;
			public float Time;
		}

		[SerializeField, ReadOnly] // И ТАК И С ХАЙДИСПЕКТОР НЕ ПОЛУЧАЕТСЯ КОНТЕКСТНОЕ МЕНЮ ВЫЗВАТЬ. В стандарте работает.
		[Tooltip("Для заполнения используйте контекстное меню, на этом файле нажмите правой мышкой \"Create File\"")]
		private TextAsset _json;

		//HideInInspector, SerializeField]
		//public List<Data> Records { get; private set; }  // Долго мучился, развекрнул ниже и точки стали сохраняться.

		[HideInInspector, SerializeField]
		private List<Data> _records;
		public List<Data> Records
		{
			get => _records ?? (_records = new List<Data>(10));
			private set => _records = value;
		}


		private void Awake()
		{
			//todo comment: Что будет, если в теле этого условия не сделать выход из метода?
			/* 
			Если убрать ретурн, как я понял, то Юнити даст такую ошибку и ошибку с просьбой о том что написано в log'e
			
			--- UnassignedReferenceException: The variable _json of PositionSaver has not been assigned.
			You probably need to assign the _json variable of the PositionSaver script in the inspector.
			UnityEngine.TextAsset.get_text () (at <2d8783c7af0442318483a199a473c55b>:0)
			DefaultNamespace.PositionSaver.Awake () (at Assets/Scripts/PositionSaver.cs:30)
			... А с рэтурном только просьба из лога - создать джейсон.			
			*/
			if (_json == null)
			{
				gameObject.SetActive(false);
				Debug.LogError("Please, create TextAsset and add in field _json"); // Создал в папке: \Assets\Jsons\TextAsset.json
				return;
			}

			JsonUtility.FromJsonOverwrite(_json.text, this);
			//todo comment: Для чего нужна эта проверка (что она позволяет избежать)?
			// Она устраняет сбой, если нет в джейсоне Рекордс.
			if (Records == null)
				Records = new List<Data>(10);
		}

		private void OnDrawGizmos()
		{
			//todo comment: Зачем нужны эти проверки (что они позволляют избежать)?
				// проверяет есть ли список Рекордс и или есть ли элемент.
			if (Records == null || Records.Count == 0) return;
			var data = Records;
			var prev = data[0].Position;
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(prev, 0.3f);
			//todo comment: Почему итерация начинается не с нулевого элемента?
				// Наверно потому что начальная точку замерять не надо.
			for (int i = 1; i < data.Count; i++)
			{
				var curr = data[i].Position;
				Gizmos.DrawWireSphere(curr, 0.3f);
				Gizmos.DrawLine(prev, curr);
				prev = curr;
			}
		}
		
#if UNITY_EDITOR
		[ContextMenu("Create File")]
		private void CreateFile()
		{
			//todo comment: Что происходит в этой строке?
				// Создаётся пустой файл "Path.txt" в папке Assets проекта.				 
			var stream = File.Create(Path.Combine(Application.dataPath, "Path.txt"));
			//todo comment: Подумайте для чего нужна эта строка? (а потом проверьте догадку, закомментировав)
				// stream.Dispose() закрывает файловый поток. Без этого файл создастся, но может возникнуть проблемы при его изменении/удалении.
			stream.Dispose();
			UnityEditor.AssetDatabase.Refresh();
			//В Unity можно искать объекты по их типу, для этого используется префикс "t:"
			//После нахождения, Юнити возвращает массив гуидов (которые в мета-файлах задаются, например)
			var guids = UnityEditor.AssetDatabase.FindAssets("t:TextAsset");
			foreach (var guid in guids)
			{
				//Этой командой можно получить путь к ассету через его гуид
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
				//Этой командой можно загрузить сам ассет
				var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(path);
				//todo comment: Для чего нужны эти проверки?
					// Без этого при обращении к asset.name может возникнуть NullReferenceException.
				if (asset != null && asset.name == "Path")
				{
					_json = asset;
					UnityEditor.EditorUtility.SetDirty(this);
					UnityEditor.AssetDatabase.SaveAssets();
					UnityEditor.AssetDatabase.Refresh();
					//todo comment: Почему мы здесь выходим, а не продолжаем итерироваться?
						// Потому что задача метода — найти и назначить ОДИН конкретный файл ("Path").
					return;
				}
			}
		}

		private void OnDestroy()
		{
			if (_json != null)			
			{				
				string json = JsonUtility.ToJson(this);
				File.WriteAllText(UnityEditor.AssetDatabase.GetAssetPath(_json), json);
				UnityEditor.AssetDatabase.ImportAsset(UnityEditor.AssetDatabase.GetAssetPath(_json));
			}
			Debug.Log($"PositionSaver на объекте {gameObject.name} уничтожен.");
}

#endif
	}
}