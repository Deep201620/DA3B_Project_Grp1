using System;

class HelloWorld {

  static void Main() {
        int maths = Convert.ToInt32(Console.ReadLine());
        int physics = Convert.ToInt32(Console.ReadLine());
        int chemistry = Convert.ToInt32(Console.ReadLine());
        int total = 0;
        int total_of_m_p = 0;
        if(maths>=65 && physics >= 55 && chemistry >= 50){
            total = maths + physics + chemistry;
            total_of_m_p = maths + physics;
            if (total >= 180 || total >= 140){
                Console.WriteLine("The total of Maths, physics and chemistry: {0}",total);
                Console.WriteLine("the total of Maths and Physics: {0}",total_of_m_p);
                Console.Write("The candidate is eligible for admission.\n");
            }
            else{
                Console.Write("The  candidate is NOT eligible for admission.\n");
            }
        }

  }
}
