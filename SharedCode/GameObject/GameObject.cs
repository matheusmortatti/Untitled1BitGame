using System;
using System.Collections.Generic;
using System.Text;
using SharedCode.Physics;
using SharedCode.Graphics;
using SharedCode.Input;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace SharedCode {
	public class GameObject {
		private List<Component> components = new List<Component>();

		public bool isPaused { get; set; }

		public float depth { get; protected set; }
		public Transform transform { get; protected set; }

		public double lifeTime { get; set; }

		private Box _collisionBox;

		public int id { get; private set; }
		public Box collisionBox {
			get { return _collisionBox; }
			set {
				_collisionBox?.CleanUp();
				_collisionBox = value;
				if (_collisionBox != null) _collisionBox.gameObject = this;
			}
		}

		/// <summary>
		/// Defines if object should stop existing or not.
		/// </summary>

		private bool _done;
		public bool done {
			get {
				return _done;
			}
			set {
				_done = value;
			}
		}

		private List<string> _tags;
		public List<string> tags {
			get {
				return new List<string>(_tags);
			}
			set {
				GameObjectManager.RemoveFromTagList(this);
				_tags = new List<string>(value);
				GameObjectManager.InsertInTagList(this);
			}
		}
		public List<string> ignoreSolidCollision;

		public List<GameObject> collidedLast { get; protected set; }
		public List<GameObject> collidedAlready { get; protected set; }
		public List<GameObject> collidedEnterAlready { get; protected set; }
		public List<GameObject> collidedExitAlready { get; protected set; }

		/// <summary>
		/// Fade out variables.
		/// </summary>
		private double foTimePassed;
		public double fadeOutTime;
		public bool fadeOut;
		private int fillp;
		private static int lastId = -1;

		private Dictionary<string, MethodInfo> states;
		public string currentState { get; private set; }
		private const BindingFlags BINDING_FLAGS = BindingFlags.NonPublic | BindingFlags.Instance;

		public GameObject(Vector2 position, params Component[] comps) {
			components.AddRange(comps);

			transform = new Transform(position);
			collisionBox = null;

			collidedLast = new List<GameObject>();
			collidedAlready = new List<GameObject>();
			collidedEnterAlready = new List<GameObject>();
			collidedExitAlready = new List<GameObject>();

			ignoreSolidCollision = new List<string>();
			_tags = new List<string>();

			depth = position.Y;

			lastId += 1;
			id = lastId;

			//
			// Setup states
			//

			states = new Dictionary<string, MethodInfo>();

			var ss = GetType().GetMethods(BINDING_FLAGS);
			foreach (var s in ss) {
				if (s.Name.Contains("StateUpdate")) {
					states[s.Name] = s;
				}
				else if (s.Name.Contains("StateInit")) {
					states[s.Name] = s;
				}
			}
		}

		public GameObject(Vector2 position, Box collisionBox, params Component[] comps)
				: this(position, comps) {
			this.collisionBox = collisionBox;
			if (collisionBox != null) this.collisionBox.position = transform.position;
		}

		#region States

		public void InitState(string stateName) {
			if (!states.ContainsKey(stateName + "StateUpdate") && !states.ContainsKey(stateName + "StateInit")) {
				throw new ArgumentException($"{stateName} is not a valid state!");
			}

			if (states.ContainsKey(stateName + "StateInit"))
				states[stateName + "StateInit"].Invoke(this, new object[] { currentState });

			this.currentState = stateName;
		}

		#endregion

		public T GetComponent<T>() where T : Component {
			foreach (var c in components) {
				if (c is T) {
					return (T)c;
				}
			}

			return default;
		}

		public List<T> GetComponents<T>() where T : Component {
			List<T> res = new List<T>();
			foreach (var c in components) {
				if (c is T) {
					res.Add((T)c);
				}
			}

			return res;
		}


		/// <summary>
		/// Returns whether or not the game object should stop moving
		/// after it collides to another object.
		/// </summary>
		public bool shouldIgnoreSolidCollision(GameObject other) {
			if (other == null) return false;

			foreach (var t in ignoreSolidCollision) {
				if (other.tags.Contains(t)) return true;
			}

			return false;
		}

		public Component AddComponent(Component component) {
			components.Add(component);
			return component;
		}

		public T RemoveComponent<T>() {
			for (int i = components.Count - 1; i >= 0; i--) {
				if (components[i] is T) {
					var comp = components[i];
					components.RemoveAt(i);
					try {
						return (T)((object)comp);
					}
					catch (InvalidCastException) {
						return default;
					}
				}
			}

			return default;
		}

		public Component RemoveComponent(Component component) {
			components.Remove(component);
			return component;
		}

		public virtual void Update(GameTime gameTime) {
			for (int i = components.Count - 1; i >= 0; --i) {
				components[i].Update(this, gameTime);
			}

			List<GameObject> collidedWith = GetComponent<APhysics>()?.collidedWith;

			if (collidedWith != null) {
				// Call collision code.
				foreach (var other in collidedWith) {
					if (!collidedLast.Contains(other)) {
						if (!collidedEnterAlready.Contains(other)) {
							OnCollisionEnter(other);
							other.OnCollisionEnter(this);

							other.collidedEnterAlready.Add(this);
						}
					}

					if (!collidedAlready.Contains(other)) {
						OnCollision(other);
						other.OnCollision(this);

						other.collidedAlready.Add(this);
					}
				}

				foreach (var other in collidedLast) {
					if (!collidedWith.Contains(other)) {
						if (!collidedExitAlready.Contains(other)) {
							OnCollisionExit(other);
							other.OnCollisionExit(this);

							other.collidedExitAlready.Add(this);
						}
					}
				}

				collidedLast.Clear();
				collidedLast.AddRange(collidedWith);
				collidedLast.AddRange(collidedAlready);
				collidedAlready.Clear();
				collidedEnterAlready.Clear();
				collidedExitAlready.Clear();
			}

			if (fadeOut) {
				fillp = 0b1010010110100101;
			}

			if (states.ContainsKey(currentState + "StateUpdate"))
				states[currentState + "StateUpdate"].Invoke(this, new object[] { gameTime });
		}

		public virtual void Draw() {
			GameObjectManager.Fillp(fillp);
			foreach (var obj in GetComponents<AGraphics>()) {
				obj.Draw(this);
			}
			GameObjectManager.Fillp();

			if (Debug.debugMode) {
				collisionBox?.DrawCollisionBox();
			}
		}

		public virtual void OnCollision(GameObject other) { }
		public virtual void OnCollisionEnter(GameObject other) { }
		public virtual void OnCollisionExit(GameObject other) { }

		public virtual void OnDestroy() { }

		public virtual void CleanUp() {
			_collisionBox?.CleanUp();

			Misc.TaskScheduler.RemoveTasksUnderId(id);
		}
	}
}
