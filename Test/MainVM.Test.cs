using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace SLD.Tezos.Client
{
	using Protocol;
	using UI;
	using Tools;
	using Model;

	[TestClass]
	public class MainVMTest : ApplicationTest
	{
		private MainVM Main;

		PropertyMonitor monitor;

		[TestInitialize]
		public async Task BeforeEach()
		{
			await ConnectToSimulation();

			Main = new MainVM(Engine);

			monitor = new PropertyMonitor(Main);
		}

		[TestMethod]
		public void Main_Initialized()
		{
			Assert.AreSame(Main, MainVM.Current);
			Assert.AreSame(Engine, Main.Engine);

			Assert.IsTrue(Main.CanSendTransactions);

			Assert.IsFalse(Main.Identities.Any());
			Assert.IsNull(Main.CurrentIdentity);
			Assert.IsFalse(Main.HasMultipleIdentities);
			Assert.IsTrue(Main.HasNoIdentity);

			Assert.AreEqual(ServiceState.Operational, Main.ServiceState);
		}

		[TestMethod]
		public async Task Main_Identities()
		{
			var identity = await Engine.AddIdentity("Test", "Test", "Test");

			await identity.WhenInitialized;

			Assert.AreEqual(1, Main.Identities.Count());
			Assert.AreSame(identity, (Identity)Main.CurrentIdentity);
			Assert.IsFalse(Main.HasMultipleIdentities);
			Assert.IsFalse(Main.HasNoIdentity);

			monitor.AssertOnce(nameof(MainVM.CurrentIdentity));
			monitor.AssertOnce(nameof(MainVM.HasNoIdentity));

			monitor.Clear();

			identity = await Engine.AddIdentity("Test", "Test", "Test");

			Assert.AreEqual(2, Main.Identities.Count());
			Assert.IsTrue(Main.HasMultipleIdentities);

			monitor.AssertOnce(nameof(MainVM.HasMultipleIdentities));
		}
	}
}