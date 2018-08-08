namespace SLD.Tezos.Client.UI
{
	public abstract class ViewModel : ClientObject
	{
		public const string TempPassphrase = " ";

		public virtual Engine Engine => Main.Engine;

		public MainVM Main => MainVM.Current;
	}

	public class SubViewModel : ViewModel
	{
		public SubViewModel(ViewModel parent)
		{
			this.Parent = parent;
		}

		public ViewModel Parent { get; private set; }

		public override Engine Engine => Parent.Engine;
	}
}