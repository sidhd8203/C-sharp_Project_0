using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Media;


namespace SmartRiceCooker00
{
    enum CookerProcess { None, Riceing, Watering, Washing, Droping, Cooking, Completion, Keeping };    
    struct RiceCookerInfo
    {
        public bool CoverOpenClose; // 뚜껑 열기 닫기
        public bool Power;        
        public CookerProcess State;               
        public int Rice;   // 쌀의 양 g 기준, 출력 때 환산
        public int Water;  // 물의 양 미리 리터 기준, 출력때 환산
        public int Number; // 인원수

        public RiceCookerInfo(int _Rice, int _Water)
        {
            Rice = _Rice;
            Water = _Water;
            State = CookerProcess.None;           
            CoverOpenClose = false;
            Power = false;           
            Number = 0;
        }
    }

    class Program
    {
        public static int MainMenuIndex = 0; // 메인 메뉴 선택 인덱스

        // Note: 밥솥 출력 메서드
        static void RiceCookerBox(int x, int y)
        {
            int height = 20;
            Console.SetCursorPosition(x, y);
            Console.Write("┌──────────────────────┐");
            for (int i = 1; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("│                                            │");
            }

            Console.SetCursorPosition(x, y + height);
            Console.Write("└──────────────────────┘");
        }

        // Note: 쌀통과 물통 출력 메서드
        static void RiceOrWaterBox(int x, int y)
        {
            int height = 20;
            Console.SetCursorPosition(x, y);
            Console.Write("┌──────────┐");
            for (int i = 1; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("│                    │");
            }

            Console.SetCursorPosition(x, y + height);
            Console.Write("└──────────┘");
        }

        // Note: 쌀 출력 메서드
        static void RiceHeight(int x, int y, int Amount)
        {
            int Height = Amount / 1000;
            // 지우는 부분
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = 0; i < 18; i++)
            {
                Console.SetCursorPosition(x, 2 + i);
                Console.Write("                    ");
            }

            for (int i = 0; i < Height ; i++)
            {
                Console.SetCursorPosition(x, 19 - i);
                Console.Write("◎◎◎◎◎◎◎◎◎◎");
            }
        }

        // Note: 물 출력 메서드
        static void WaterHeight(int x, int y, int Amount)
        {
            int Height = Amount / 1000;
            // 지우는 부분
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = 0; i < 18; i++)
            {
                Console.SetCursorPosition(x, 2 + i);
                Console.Write("                    ");
            }
            
            Console.BackgroundColor = ConsoleColor.Blue;
            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(x, 19 - i);
                Console.Write("                    ");
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        // Note: 밥솥 상태 정보 박스와 메뉴 박스 출력 메서드
        static void InfoOrMenuBox(int x, int y)
        {
            int height = 13;
            Console.SetCursorPosition(x, y);
            Console.Write("┌──────────────────────┐");
            for (int i = 1; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("│                                            │");
            }

            Console.SetCursorPosition(x, y + height);
            Console.Write("└──────────────────────┘");
        }

        // Note: 뚜껑 열기 닫기 출력 메서드
        static void Cover(bool bOpen)
        {
            const int x = 16;
            if (bOpen)
            {
                Console.SetCursorPosition(x, 3);
                Console.Write("┌┐");
                for(int i = 0; i < 6; i++)
                {
                    Console.SetCursorPosition(x, 4 + i);
                    Console.Write("││");
                }               
                Console.SetCursorPosition(x, 10);
                Console.Write("└┘");               
            }
            else
            {
                Console.SetCursorPosition(x, 9);
                Console.Write("┌──────────┐");
                Console.SetCursorPosition(x, 10);
                Console.Write("└──────────┘");                
            }                              
        }

        // Note: 밥솥 출력 메서드
        static void RiceBox(int x, int y)
        {
            int height = 7;
            Console.SetCursorPosition(x, y);
            Console.Write("┌──────────┐");
            for (int i = 1; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("│                    │");
            }

            Console.SetCursorPosition(x, y + height);
            Console.Write("└──────────┘");
            Console.SetCursorPosition(x + 10, y + 2);
            Console.Write("炊飯器");
            Console.SetCursorPosition(x, y + 6);
            Console.Write("┤"); // 전원 부분
        }

        static void RiceInfo(RiceCookerInfo Info)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(3, 25);
            if (Info.Power)// false
                Console.Write("電源 : ON");
            else
                Console.Write("電源 : OFF");

            Console.SetCursorPosition(3, 26);
            if (Info.CoverOpenClose)//false
                Console.Write("フタ : 開き");　
            else
                Console.Write("フタ : 閉じ");

            Console.SetCursorPosition(3, 27);
            switch(Info.State)
            {
                case CookerProcess.None:
                    Console.Write("炊飯器状態 : 待機中  ");
                    break;
                case CookerProcess.Riceing:
                    Console.Write("炊飯器状態 : 米入れ  ");
                    break;
                case CookerProcess.Watering:
                    Console.Write("炊飯器状態 : 水入れ  ");
                    break;
                case CookerProcess.Washing:
                    Console.Write("炊飯器状態 : 米洗い  ");
                    break;
                case CookerProcess.Droping:
                    Console.Write("炊飯器状態 : 水排水  ");
                    break;   
                case CookerProcess.Cooking:
                    Console.Write("炊飯器状態 : 炊く中  ");
                    break;
                case CookerProcess.Completion:
                    Console.Write("炊飯器状態 : 炊く完了");
                    break;
                case CookerProcess.Keeping:
                    Console.Write("炊飯器状態 : 保温中  ");
                    break;
            }

            Console.SetCursorPosition(3, 28); 
            Console.Write("人数 : {0}", Info.Number);
            Console.SetCursorPosition(3, 29);
            Console.Write("米状態 : {0:f1} Kg", Info.Rice / 1000.0f);
            Console.SetCursorPosition(3, 30);
            Console.Write("水状態 : {0:f1} リッター", Info.Water / 1000.0f);
        }

        static void MessageBox(int x, int y, string Message)
        {
            int height = 3;
            Console.SetCursorPosition(x, y);
            Console.Write("┌───────────────────┐");
            for (int i = 1; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("│                                      │");
            }

            Console.SetCursorPosition(x, y + height);
            Console.Write("└───────────────────┘");
            Console.SetCursorPosition(x + 2, y + 1);
            Console.Write(Message);
        }

        static void OutFrame()
        {            
            RiceCookerBox(0, 0);
            RiceOrWaterBox(48, 0);
            RiceOrWaterBox(72, 0);
            InfoOrMenuBox(0, 21);
            InfoOrMenuBox(48, 21);
            Console.SetCursorPosition(17, 1);
            Console.Write("Smart 炊飯器");
            Console.SetCursorPosition(58, 1);
            Console.Write("米筒");
            Console.SetCursorPosition(82, 1);
            Console.Write("水筒");
            Console.SetCursorPosition(18, 23);
            Console.Write("炊飯器情報");
            Console.SetCursorPosition(66, 23);
            Console.Write("(( メニュー ))");            
        }

        static void Menu(int x, int y, string[] MenuItem)
        {
            ConsoleKeyInfo InputKey;           
           
            while(true)
            {
                for(int i = 0; i < MenuItem.Length; i++)
                {
                    if(MainMenuIndex == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.SetCursorPosition(x, y + i);
                        Console.Write(MenuItem[i]);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.SetCursorPosition(x, y + i);
                        Console.Write(MenuItem[i]);
                    }
                }

                InputKey = Console.ReadKey(true);
                if (InputKey.Key == ConsoleKey.Enter)
                    break;
                else if(InputKey.Key == ConsoleKey.UpArrow)
                {
                    MainMenuIndex--;
                    if (MainMenuIndex < 0)
                        MainMenuIndex = 0;
                }else if(InputKey.Key == ConsoleKey.DownArrow)
                {
                    MainMenuIndex++;
                    if (MainMenuIndex == MenuItem.Length)
                        MainMenuIndex = MenuItem.Length - 1;
                }
            }          
        }       

        static void PowerLine(bool Power)
        {
            if(Power)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(4, 17);
                Console.Write("──────");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;               
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(4, 17);
                Console.Write("──────");
            }
        }
       

        static void Main(string[] args)
        {           
            Console.SetWindowSize(99, 36);
            RiceCookerInfo RCInfo = new RiceCookerInfo(10000, 5000);
            SoundPlayer Sound = new SoundPlayer();

            string[] MenuItem = { "   電源   ", "   フタ   ", "   炊く   ", "   保温   ", "   キャンセル   ",
                                    "  人数  ", "    米   ", "    水    "  };
            while (true)
            {
                OutFrame();
                RiceBox(16, 11);
                Cover(RCInfo.CoverOpenClose);
                RiceInfo(RCInfo);
                PowerLine(RCInfo.Power);
                RiceHeight(50, 2, RCInfo.Rice);
                WaterHeight(74, 2, RCInfo.Water);

                Menu(65, 25, MenuItem);
                if (MainMenuIndex == 9)
                    break;

                switch (MainMenuIndex)
                {
                    case 0: // 전원
                        RCInfo.Power = !RCInfo.Power;
                        if(RCInfo.Power)
                        {
                            Sound.SoundLocation = "power_on.wav";
                        }
                        else
                        {
                            Sound.SoundLocation = "power_off.wav";
                        }                        
                        Sound.Load();
                        Sound.PlaySync();
                        break;
                    case 1: // 뚜껑, 취사 중에 뚜껑이 열리면 안된다.
                        if (RCInfo.State == CookerProcess.Cooking)
                        {
                            MessageBox(51, 27, " 炊く中の際にはフタを開けられないです");
                            Console.ReadKey(true);
                        }
                        else
                        {
                            RCInfo.CoverOpenClose = !RCInfo.CoverOpenClose;
                            if (RCInfo.CoverOpenClose)
                                Sound.SoundLocation = "cover_open.wav";
                            else
                                Sound.SoundLocation = "cover_close.wav";
                            Sound.Load();
                            Sound.Play();
                        }
                        break;
                    case 2: // 취사
                        if (!RCInfo.Power)
                        {
                            // 밧데리로 일부 메시지 전달
                            MessageBox(51, 27, "電源が消えています");
                            Console.ReadKey(true);
                            break;
                        }

                        if (RCInfo.CoverOpenClose)
                        {
                            // 밧데리로 일부 메시지 전달
                            MessageBox(51, 27, "フタが開けてないです");
                            Console.ReadKey(true);
                            break;
                        }

                        if (RCInfo.Number == 0)
                        {
                            // 밧데리로 일부 메시지 전달
                            MessageBox(51, 27, "人数を入力してください");
                            Console.ReadKey(true);
                            break;
                        }

                        // 일정한 시간 간격으로 쌀 넣기 -> 물 넣기 -> 쌀 씻기 -> 배수 -> 2 번 반복, 물 넣기부터 
                        //                     취사 -> 완료 -> 보온                       
                        // 필요한 쌀과 물의 공급이 되어 있는지를 체크한다.
                        int Rice = RCInfo.Rice - (RCInfo.Number * 160); // 쌀 일인분 160g
                        if (Rice < 0)
                        {
                            MessageBox(51, 27, "  ??? 米不足 ???");
                            Sound.SoundLocation = "쌀을보충해주세요.WAV";
                            Sound.Load();
                            Sound.Play();
                            Console.ReadKey(true);
                            break;
                        }

                        //  물통에서 물 빼기 (대략 인원수 x 170 ml ) * 5; //물로 씻는 거 2번 취사 1번 총 3번 양이 필요
                        //  씻을 때는 1인분 물 170의 두 배 사용, 필요한 물은 씻기 2번(170*4 ml) 취사 1 번(170 ml)
                        int Water;
                        Water = RCInfo.Water - (RCInfo.Number * 170)*5;
                        if (Water < 0)
                        {
                            MessageBox(51, 27, "  ??? 水不足 ???");
                            Sound.SoundLocation = "물보충.WAV";
                            Sound.Load();
                            Sound.Play();
                            Console.ReadKey(true);
                            break;
                        }

                        // Note: 취사 시작 부분, 쌀 넣기 -> 물 넣기 -> 쌀 씻기 -> 배수 -> 취사 -> 보온
                        RCInfo.State = CookerProcess.Riceing;
                        RiceInfo(RCInfo);

                        Sound.SoundLocation = "쌀넣기.WAV";
                        Sound.Load();
                        Sound.Play();

                        Console.SetCursorPosition(24, 12);
                        Console.Write("米入れ");
                        Console.SetCursorPosition(18, 13);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        Console.SetCursorPosition(18, 14);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        Console.SetCursorPosition(18, 15);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        Console.SetCursorPosition(18, 16);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        Console.SetCursorPosition(18, 17);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        RCInfo.Rice = RCInfo.Rice - (RCInfo.Number * 160); // 1인분 160g
                        RiceHeight(50, 2, RCInfo.Rice);
                        Thread.Sleep(3000); // 3초 정도                            
                        
                        for (int i = 0; i < 2; i++)
                        {
                            // Note: 물 넣기 --> 파란 색 보여 주기
                            RCInfo.State = CookerProcess.Watering;
                            RCInfo.Water = RCInfo.Water - (RCInfo.Number * 170 * 2);
                            RiceInfo(RCInfo);

                            Sound.SoundLocation = "water_in.WAV";
                            Sound.Load();
                            Sound.Play();

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(24, 12);
                            Console.Write("水入れ");
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.SetCursorPosition(18, 13);
                            Console.Write("◎◎◎◎◎◎◎◎◎◎");
                            Console.SetCursorPosition(18, 14);
                            Console.Write("◎◎◎◎◎◎◎◎◎◎");
                            Console.SetCursorPosition(18, 15);
                            Console.Write("◎◎◎◎◎◎◎◎◎◎");
                            Console.SetCursorPosition(18, 16);
                            Console.Write("◎◎◎◎◎◎◎◎◎◎");
                            Console.SetCursorPosition(18, 17);
                            Console.Write("◎◎◎◎◎◎◎◎◎◎");                            
                            WaterHeight(74, 2, RCInfo.Water);
                            Thread.Sleep(3000); // 3초 정도

                            // Note: 쌀 씻기 
                            Sound.SoundLocation = "쌀씻기.wav";
                            Sound.Load();
                            Sound.Play();
                            RCInfo.State = CookerProcess.Washing;
                            RiceInfo(RCInfo);

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(24, 12);                           
                            Console.Write("米洗い");
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.SetCursorPosition(18, 13);
                            Console.Write("~ ~ ~ ~ ~ ~ ~ ~ ~ ~");
                            Console.SetCursorPosition(18, 14);
                            Console.Write("◎◎◎◎◎◎◎◎◎◎");
                            Console.SetCursorPosition(18, 15);
                            Console.Write("~ ~ ~ ~ ~ ~ ~ ~ ~ ~");
                            Console.SetCursorPosition(18, 16);
                            Console.Write("◎◎◎◎◎◎◎◎◎◎");
                            Console.SetCursorPosition(18, 17);
                            Console.Write("~ ~ ~ ~ ~ ~ ~ ~ ~ ~");                          
                            Thread.Sleep(3000); // 3초 정도

                            // Note: 물 배수 
                            RCInfo.State = CookerProcess.Droping;
                            RiceInfo(RCInfo);

                            Sound.SoundLocation = "water_out.WAV";
                            Sound.Load();
                            Sound.Play();

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(24, 12);
                            Console.Write(" 排水 ");
                            for (int j = 0; j < 5; j++)
                            {
                                // 지우기
                                Console.BackgroundColor = ConsoleColor.Black;
                                for (int k = 0; k < j; k++)
                                {
                                    Console.SetCursorPosition(18, 13 + k);
                                    Console.Write("◎◎◎◎◎◎◎◎◎◎");
                                }

                                // 물 출력
                                Console.BackgroundColor = ConsoleColor.Blue;
                                for (int k = j; k < 5; k++)
                                {
                                    Console.SetCursorPosition(18, 13 + k);
                                    Console.Write("◎◎◎◎◎◎◎◎◎◎");
                                }
                                Thread.Sleep(700);                               
                            }
                            
                        }

                        // Note: 취사용 물 공급
                        RCInfo.Water = RCInfo.Water - (RCInfo.Number * 170);                        
                        WaterHeight(74, 2, RCInfo.Water);
                        RiceInfo(RCInfo);

                        // Note: 취사 시작
                        RCInfo.State = CookerProcess.Cooking;
                        RiceInfo(RCInfo);

                        Sound.SoundLocation = "rice.wav";
                        Sound.Load();
                        Sound.Play();

                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.SetCursorPosition(24, 12);
                        Console.Write("炊く中");
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(18, 13);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        Console.SetCursorPosition(18, 14);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        Console.SetCursorPosition(18, 15);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        Console.SetCursorPosition(18, 16);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        Console.SetCursorPosition(18, 17);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        Thread.Sleep(7000); // 7초 정도

                        // Note: 완료 , 사운드 삐리릭...
                        RCInfo.State = CookerProcess.Completion;
                        RiceInfo(RCInfo);
                        Sound.SoundLocation = "Ring10.wav";
                        Sound.Load();
                        Sound.Play();
                        Thread.Sleep(7000); // 3초 정도

                        Sound.SoundLocation = "밥완료.wav";
                        Sound.Load();
                        Sound.Play();

                        Console.SetCursorPosition(24, 12);
                        Console.Write("炊く完了");
                        Thread.Sleep(3000); // 3초 정도
                        
                        // Note: 보온
                        RCInfo.State = CookerProcess.Keeping;
                        RiceInfo(RCInfo);

                        Sound.SoundLocation = "맛있게드세요.wav";
                        Sound.Load();
                        Sound.Play();

                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.SetCursorPosition(24, 12);
                        Console.Write("保温中  ");                        
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(18, 13);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        Console.SetCursorPosition(18, 14);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        Console.SetCursorPosition(18, 15);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        Console.SetCursorPosition(18, 16);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        Console.SetCursorPosition(18, 17);
                        Console.Write("◎◎◎◎◎◎◎◎◎◎");
                        Thread.Sleep(3000); // 3초 정도
                        Console.ForegroundColor = ConsoleColor.White;

                        RCInfo.Number = 0; // Note: 인원수 초기화
                        break;
                    case 3: // Note:보온
                        if (!RCInfo.Power)
                        {
                            // 밧데리로 일부 메시지 전달
                            MessageBox(51, 27, "電源が消えています");
                            Console.ReadKey(true);
                            break;
                        }

                        RCInfo.State = CookerProcess.Keeping;
                        RiceInfo(RCInfo);
                        break;
                    case 4: // 취소
                        RCInfo.State = CookerProcess.None;                        
                        RiceInfo(RCInfo);
                        break;                   
                    case 5: // 인원수
                        if (!RCInfo.Power)
                        {
                            // 밧데리로 일부 메시지 전달
                            MessageBox(51, 27, "電源が消えています");
                            Console.ReadKey(true);
                            break;
                        }

                        MessageBox(51, 27, " 食事する人数 : ");
                        try {
                            RCInfo.Number = int.Parse(Console.ReadLine());
                        }catch(Exception e)
                        {
                            RCInfo.Number = 0;
                        }
                        break;

                    case 6: // 쌀통 설정
                        {                            
                            string Message = "現在米の量(kg) : " + (RCInfo.Rice / 1000);
                            MessageBox(51, 27, Message);                           
                            Console.SetCursorPosition(63, 29);
                            Console.Write("追加する米の量(kg) : ");
                            string Amount = Console.ReadLine();
                            try
                            {
                                RCInfo.Rice += int.Parse(Amount) * 1000; // kg 단위
                                if(RCInfo.Rice > 18000) // 18kg 최대
                                {
                                    RCInfo.Rice -= int.Parse(Amount) * 1000;
                                    MessageBox(51, 27, "量が多すぎです");
                                    Console.ReadKey(true);
                                    break;
                                }
                            }catch(Exception e)
                            {
                                break;
                            }                          
                        }
                        break;
                    case 7: // 뭍통 설정
                        {
                            string Message = "現在水の量(リッター) : " + (RCInfo.Water / 1000);
                            MessageBox(51, 27, Message);
                            string Amount = string.Empty;
                            Console.SetCursorPosition(63, 29);
                            Console.Write("追加する水の量(リッター) : ");
                            Amount = Console.ReadLine();
                            try
                            {
                                RCInfo.Water += int.Parse(Amount) * 1000; // 리터를 밀리리터로 
                                if(RCInfo.Water > 18000)
                                {
                                    RCInfo.Water -= int.Parse(Amount) * 1000;
                                    MessageBox(51, 27, "量が多すぎです");
                                    Console.ReadKey(true);
                                    break;
                                }                                
                            }catch(Exception e)
                            {
                                break;
                            }                            
                        }
                        break;
                }          
            }
        }
    }
}
