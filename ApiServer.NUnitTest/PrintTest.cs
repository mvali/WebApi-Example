using Moq;
using NUnit.Framework;
using System;

namespace ApiServer.NUnitTest
{
    // posted here for fast inspection and quick access to object details
    public interface IPrinterStatus { } // used as mock on functions for a bit more testing cases
    public interface IPrinter { void Print(int answer, IPrinterStatus status); }
    public class Printer : IPrinter
    {
        public void Print(int answer, IPrinterStatus status) { }// => Console.WriteLine("The answer is {0}.", answer);
    }
    public class PrintService
    {
        private IPrinter printer;
        public PrintService(IPrinter printer) => this.printer = printer;

        public void MakeCopies(int sheets1, int sheets2, IPrinterStatus status)
        {
            printer.Print(sheets1 + sheets2, status);
        }
    }
    class PrintTest
    {
        [Test]
        public void MakeCopiesCalled__ShouldCallPrint()
        {
            /* Arrange */
            var iPrinterMock = new Mock<IPrinter>();
            var PrinterStatusMock = new Mock<IPrinterStatus>(); // It.IsAny<IPrinterStatus>()

            // Let's mock the method so when it is called, we handle it
            iPrinterMock.Setup(x => x.Print(It.IsAny<int>(), PrinterStatusMock.Object));

            // Create the printservice and pass the mocked printer to it
            var pRservice = new PrintService(iPrinterMock.Object);

            /* Act print once */
            pRservice.MakeCopies(1, 1, PrinterStatusMock.Object);

            /* Assert */
            // Let's make sure that the calculator's Add method called printer.Print. Here we are making sure it is called once but this is optional
            iPrinterMock.Verify(x => x.Print(It.IsAny<int>(), PrinterStatusMock.Object), Times.Once);

            // Or we can be more specific and ensure that Print was called with the correct parameter.
            // value of Print parameter must match the parameter used in Print otherwise will fail
            iPrinterMock.Verify(x => x.Print(2, PrinterStatusMock.Object), Times.Exactly(1));
        }
    }
}
