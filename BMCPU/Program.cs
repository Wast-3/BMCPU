using System;
using System.IO;

namespace BMCPU
{
    class Program
    {
        static void Main(string[] args)
        {
            var CPU1 = new CPU();
            Console.WriteLine("CPU Initalized");
            Console.WriteLine("Memory Initalized");
            Console.WriteLine("Registers initalized");
            Console.WriteLine("Register Dump: ");
            CPU1.RegisterState.DebugRegisters();
            Console.WriteLine("Reading Boot Disk");
            var CurrentDisk = new Disk();
            CurrentDisk.DebugDisk();
            CPU1.WriteDiskToMemory(CurrentDisk);
            Console.WriteLine("Memory Dump: ");
            CPU1.MemoryState.DebugMemory();
            Console.WriteLine("Beginning Execution");
            CPU1.BeginExecution(0x00);
        }
    }
    class Disk
    {
        public Byte[] Program { get; set; }

        public Disk()
        {
            this.Program = File.ReadAllBytes("");
        }
        public void DebugDisk()
        {
            foreach (int i in Program)
            {
                Console.Write($"{i:X4} ");
            }
            Console.WriteLine();
        }
    }
    class Memory
    {
        public byte[] CPUMemory { get; set; }

        public Memory()
        {
            this.CPUMemory = new byte[255];
        }

        public void DebugMemory()
        {
            foreach (int i in CPUMemory)
            {
                Console.Write($"{i:X4} ");
            }
            Console.WriteLine();
        }
    }

    class CPU
    {
        //Control Flow Registers
        public byte PC { get; set; }
        public Registers RegisterState;
        public Memory MemoryState;
        public CPU()
        {
            this.PC = 0x00;
            this.RegisterState = new Registers();
            this.MemoryState = new Memory();
        }

        public void WriteDiskToMemory(Disk disk)
        {
            int i = 0;
            foreach (byte currentByte in disk.Program)
            {
                MemoryState.CPUMemory[i] = currentByte;
                i++;
            }
        }

        public void debug()
        {
            Console.Clear();
            RegisterState.DebugRegisters();
            MemoryState.DebugMemory();
        }

        public void BeginExecution(byte StartAddress)
        {
            PC = StartAddress;
            Execute();
        }

        private void Execute()
        {

            while (true)
            {
                System.Threading.Thread.Sleep(500);
                byte CurrentOPCode = MemoryState.CPUMemory[PC];
                Console.WriteLine($"EXECUTION: Current Address: {PC:X4}");
                Decode(CurrentOPCode);
                debug();
            }

        }

        private void Decode(byte opcode)
        {
            switch (opcode)
            {
                case 0x00: //int
                    break;
                case 0x01: // CONST -> R1
                    ImmediateToRegister(ref RegisterState.R1);
                    PC = (byte)(PC + 2);
                    break;
                case 0x02:
                    ImmediateToRegister(ref RegisterState.R2);
                    PC = (byte)(PC + 2);
                    break;
                case 0x03:
                    ImmediateToRegister(ref RegisterState.R3);
                    PC = (byte)(PC + 2);
                    break;
                case 0x04:
                    ImmediateToRegister(ref RegisterState.R4);
                    PC = (byte)(PC + 2);
                    break;
                case 0x05:
                    ImmediateToRegister(ref RegisterState.R5);
                    PC = (byte)(PC + 2);
                    break;
                case 0x06:
                    RegisterState.MEM1 = MemoryState.CPUMemory[PC + 1];
                    PC = (byte)(PC + 2);
                    break;
                case 0x07:
                    RegisterState.MEM2 = MemoryState.CPUMemory[PC + 1];
                    PC = (byte)(PC + 2);
                    break;
                case 0x08:
                    MemoryState.CPUMemory[RegisterState.MEM1] = MemoryState.CPUMemory[PC + 1];
                    PC = (byte)(PC + 2);
                    break;
                case 0x09:
                    MemoryState.CPUMemory[RegisterState.MEM2] = MemoryState.CPUMemory[PC + 1];
                    PC = (byte)(PC + 2);
                    break;


                default:
                    Console.WriteLine("UNDEFINED OPCODE. TERMINATING");
                    throw new Exception("UNDEFINED OPCODE");
            }
        }

        private void ImmediateToRegister(ref byte register)
        {
            register = MemoryState.CPUMemory[PC + 1];
        }

    }
    class Registers
    {
        //General purpose registers
        public byte R1;
        public byte R2;
        public byte R3;
        public byte R4;
        public byte R5;
        //Pointer for memory-based operations
        public byte MEM1 { get; set; }
        public byte MEM2 { get; set; }
        //Result register
        public byte RES { get; set; }
        public Registers()
        {
            this.R1 = 0x00;
            this.R2 = 0x00;
            this.R3 = 0x00;
            this.R4 = 0x00;
            this.R5 = 0x00;
            this.MEM1 = 0x00;
            this.MEM2 = 0x00;
            this.RES = 0x00;
        }

        public void DebugRegisters()
        {
            Console.WriteLine("REGISTER STATES");
            Console.WriteLine($"R1: {R1:X4}");
            Console.WriteLine($"R2: {R2:X4}");
            Console.WriteLine($"R3: {R3:X4}");
            Console.WriteLine($"R4: {R4:X4}");
            Console.WriteLine($"R5: {R5:X4}");
            Console.WriteLine($"MEM1: {MEM1:X4}");
            Console.WriteLine($"MEM2: {MEM2:X4}");
        }
    }
}