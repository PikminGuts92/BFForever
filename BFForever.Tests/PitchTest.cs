﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BFForever.Riff;

namespace BFForever.Tests
{
    [TestClass]
    public class PitchTest
    {
        [TestMethod]
        public void Pitch_CastFromString_Flats()
        {
            // With flat character
            Assert.AreEqual(((Pitch)"C-1").Value, 0);
            Assert.AreEqual(((Pitch)"D♭-1").Value, 1);
            Assert.AreEqual(((Pitch)"D-1").Value, 2);
            Assert.AreEqual(((Pitch)"E♭-1").Value, 3);
            Assert.AreEqual(((Pitch)"E-1").Value, 4);
            Assert.AreEqual(((Pitch)"F-1").Value, 5);
            Assert.AreEqual(((Pitch)"G♭-1").Value, 6);
            Assert.AreEqual(((Pitch)"G-1").Value, 7);
            Assert.AreEqual(((Pitch)"A♭-1").Value, 8);
            Assert.AreEqual(((Pitch)"A-1").Value, 9);
            Assert.AreEqual(((Pitch)"B♭-1").Value, 10);
            Assert.AreEqual(((Pitch)"B-1").Value, 11);
            Assert.AreEqual(((Pitch)"C0").Value, 12);
            Assert.AreEqual(((Pitch)"D♭0").Value, 13);
            Assert.AreEqual(((Pitch)"D0").Value, 14);
            Assert.AreEqual(((Pitch)"E♭0").Value, 15);
            Assert.AreEqual(((Pitch)"E0").Value, 16);
            Assert.AreEqual(((Pitch)"F0").Value, 17);
            Assert.AreEqual(((Pitch)"G♭0").Value, 18);
            Assert.AreEqual(((Pitch)"G0").Value, 19);
            Assert.AreEqual(((Pitch)"A♭0").Value, 20);
            Assert.AreEqual(((Pitch)"A0").Value, 21);
            Assert.AreEqual(((Pitch)"B♭0").Value, 22);
            Assert.AreEqual(((Pitch)"B0").Value, 23);
            Assert.AreEqual(((Pitch)"C1").Value, 24);
            Assert.AreEqual(((Pitch)"D♭1").Value, 25);
            Assert.AreEqual(((Pitch)"D1").Value, 26);
            Assert.AreEqual(((Pitch)"E♭1").Value, 27);
            Assert.AreEqual(((Pitch)"E1").Value, 28);
            Assert.AreEqual(((Pitch)"F1").Value, 29);
            Assert.AreEqual(((Pitch)"G♭1").Value, 30);
            Assert.AreEqual(((Pitch)"G1").Value, 31);
            Assert.AreEqual(((Pitch)"A♭1").Value, 32);
            Assert.AreEqual(((Pitch)"A1").Value, 33);
            Assert.AreEqual(((Pitch)"B♭1").Value, 34);
            Assert.AreEqual(((Pitch)"B1").Value, 35);
            Assert.AreEqual(((Pitch)"C2").Value, 36);
            Assert.AreEqual(((Pitch)"D♭2").Value, 37);
            Assert.AreEqual(((Pitch)"D2").Value, 38);
            Assert.AreEqual(((Pitch)"E♭2").Value, 39);
            Assert.AreEqual(((Pitch)"E2").Value, 40);
            Assert.AreEqual(((Pitch)"F2").Value, 41);
            Assert.AreEqual(((Pitch)"G♭2").Value, 42);
            Assert.AreEqual(((Pitch)"G2").Value, 43);
            Assert.AreEqual(((Pitch)"A♭2").Value, 44);
            Assert.AreEqual(((Pitch)"A2").Value, 45);
            Assert.AreEqual(((Pitch)"B♭2").Value, 46);
            Assert.AreEqual(((Pitch)"B2").Value, 47);
            Assert.AreEqual(((Pitch)"C3").Value, 48);
            Assert.AreEqual(((Pitch)"D♭3").Value, 49);
            Assert.AreEqual(((Pitch)"D3").Value, 50);
            Assert.AreEqual(((Pitch)"E♭3").Value, 51);
            Assert.AreEqual(((Pitch)"E3").Value, 52);
            Assert.AreEqual(((Pitch)"F3").Value, 53);
            Assert.AreEqual(((Pitch)"G♭3").Value, 54);
            Assert.AreEqual(((Pitch)"G3").Value, 55);
            Assert.AreEqual(((Pitch)"A♭3").Value, 56);
            Assert.AreEqual(((Pitch)"A3").Value, 57);
            Assert.AreEqual(((Pitch)"B♭3").Value, 58);
            Assert.AreEqual(((Pitch)"B3").Value, 59);
            Assert.AreEqual(((Pitch)"C4").Value, 60);
            Assert.AreEqual(((Pitch)"D♭4").Value, 61);
            Assert.AreEqual(((Pitch)"D4").Value, 62);
            Assert.AreEqual(((Pitch)"E♭4").Value, 63);
            Assert.AreEqual(((Pitch)"E4").Value, 64);
            Assert.AreEqual(((Pitch)"F4").Value, 65);
            Assert.AreEqual(((Pitch)"G♭4").Value, 66);
            Assert.AreEqual(((Pitch)"G4").Value, 67);
            Assert.AreEqual(((Pitch)"A♭4").Value, 68);
            Assert.AreEqual(((Pitch)"A4").Value, 69);
            Assert.AreEqual(((Pitch)"B♭4").Value, 70);
            Assert.AreEqual(((Pitch)"B4").Value, 71);
            Assert.AreEqual(((Pitch)"C5").Value, 72);
            Assert.AreEqual(((Pitch)"D♭5").Value, 73);
            Assert.AreEqual(((Pitch)"D5").Value, 74);
            Assert.AreEqual(((Pitch)"E♭5").Value, 75);
            Assert.AreEqual(((Pitch)"E5").Value, 76);
            Assert.AreEqual(((Pitch)"F5").Value, 77);
            Assert.AreEqual(((Pitch)"G♭5").Value, 78);
            Assert.AreEqual(((Pitch)"G5").Value, 79);
            Assert.AreEqual(((Pitch)"A♭5").Value, 80);
            Assert.AreEqual(((Pitch)"A5").Value, 81);
            Assert.AreEqual(((Pitch)"B♭5").Value, 82);
            Assert.AreEqual(((Pitch)"B5").Value, 83);
            Assert.AreEqual(((Pitch)"C6").Value, 84);
            Assert.AreEqual(((Pitch)"D♭6").Value, 85);
            Assert.AreEqual(((Pitch)"D6").Value, 86);
            Assert.AreEqual(((Pitch)"E♭6").Value, 87);
            Assert.AreEqual(((Pitch)"E6").Value, 88);
            Assert.AreEqual(((Pitch)"F6").Value, 89);
            Assert.AreEqual(((Pitch)"G♭6").Value, 90);
            Assert.AreEqual(((Pitch)"G6").Value, 91);
            Assert.AreEqual(((Pitch)"A♭6").Value, 92);
            Assert.AreEqual(((Pitch)"A6").Value, 93);
            Assert.AreEqual(((Pitch)"B♭6").Value, 94);
            Assert.AreEqual(((Pitch)"B6").Value, 95);
            Assert.AreEqual(((Pitch)"C7").Value, 96);
            Assert.AreEqual(((Pitch)"D♭7").Value, 97);
            Assert.AreEqual(((Pitch)"D7").Value, 98);
            Assert.AreEqual(((Pitch)"E♭7").Value, 99);
            Assert.AreEqual(((Pitch)"E7").Value, 100);
            Assert.AreEqual(((Pitch)"F7").Value, 101);
            Assert.AreEqual(((Pitch)"G♭7").Value, 102);
            Assert.AreEqual(((Pitch)"G7").Value, 103);
            Assert.AreEqual(((Pitch)"A♭7").Value, 104);
            Assert.AreEqual(((Pitch)"A7").Value, 105);
            Assert.AreEqual(((Pitch)"B♭7").Value, 106);
            Assert.AreEqual(((Pitch)"B7").Value, 107);
            Assert.AreEqual(((Pitch)"C8").Value, 108);
            Assert.AreEqual(((Pitch)"D♭8").Value, 109);
            Assert.AreEqual(((Pitch)"D8").Value, 110);
            Assert.AreEqual(((Pitch)"E♭8").Value, 111);
            Assert.AreEqual(((Pitch)"E8").Value, 112);
            Assert.AreEqual(((Pitch)"F8").Value, 113);
            Assert.AreEqual(((Pitch)"G♭8").Value, 114);
            Assert.AreEqual(((Pitch)"G8").Value, 115);
            Assert.AreEqual(((Pitch)"A♭8").Value, 116);
            Assert.AreEqual(((Pitch)"A8").Value, 117);
            Assert.AreEqual(((Pitch)"B♭8").Value, 118);
            Assert.AreEqual(((Pitch)"B8").Value, 119);
            Assert.AreEqual(((Pitch)"C9").Value, 120);
            Assert.AreEqual(((Pitch)"D♭9").Value, 121);
            Assert.AreEqual(((Pitch)"D9").Value, 122);
            Assert.AreEqual(((Pitch)"E♭9").Value, 123);
            Assert.AreEqual(((Pitch)"E9").Value, 124);
            Assert.AreEqual(((Pitch)"F9").Value, 125);
            Assert.AreEqual(((Pitch)"G♭9").Value, 126);
            Assert.AreEqual(((Pitch)"G9").Value, 127);

            // With 'b' character
            Assert.AreEqual(((Pitch)"C-1").Value, 0);
            Assert.AreEqual(((Pitch)"Db-1").Value, 1);
            Assert.AreEqual(((Pitch)"D-1").Value, 2);
            Assert.AreEqual(((Pitch)"Eb-1").Value, 3);
            Assert.AreEqual(((Pitch)"E-1").Value, 4);
            Assert.AreEqual(((Pitch)"F-1").Value, 5);
            Assert.AreEqual(((Pitch)"Gb-1").Value, 6);
            Assert.AreEqual(((Pitch)"G-1").Value, 7);
            Assert.AreEqual(((Pitch)"Ab-1").Value, 8);
            Assert.AreEqual(((Pitch)"A-1").Value, 9);
            Assert.AreEqual(((Pitch)"Bb-1").Value, 10);
            Assert.AreEqual(((Pitch)"B-1").Value, 11);
            Assert.AreEqual(((Pitch)"C0").Value, 12);
            Assert.AreEqual(((Pitch)"Db0").Value, 13);
            Assert.AreEqual(((Pitch)"D0").Value, 14);
            Assert.AreEqual(((Pitch)"Eb0").Value, 15);
            Assert.AreEqual(((Pitch)"E0").Value, 16);
            Assert.AreEqual(((Pitch)"F0").Value, 17);
            Assert.AreEqual(((Pitch)"Gb0").Value, 18);
            Assert.AreEqual(((Pitch)"G0").Value, 19);
            Assert.AreEqual(((Pitch)"Ab0").Value, 20);
            Assert.AreEqual(((Pitch)"A0").Value, 21);
            Assert.AreEqual(((Pitch)"Bb0").Value, 22);
            Assert.AreEqual(((Pitch)"B0").Value, 23);
            Assert.AreEqual(((Pitch)"C1").Value, 24);
            Assert.AreEqual(((Pitch)"Db1").Value, 25);
            Assert.AreEqual(((Pitch)"D1").Value, 26);
            Assert.AreEqual(((Pitch)"Eb1").Value, 27);
            Assert.AreEqual(((Pitch)"E1").Value, 28);
            Assert.AreEqual(((Pitch)"F1").Value, 29);
            Assert.AreEqual(((Pitch)"Gb1").Value, 30);
            Assert.AreEqual(((Pitch)"G1").Value, 31);
            Assert.AreEqual(((Pitch)"Ab1").Value, 32);
            Assert.AreEqual(((Pitch)"A1").Value, 33);
            Assert.AreEqual(((Pitch)"Bb1").Value, 34);
            Assert.AreEqual(((Pitch)"B1").Value, 35);
            Assert.AreEqual(((Pitch)"C2").Value, 36);
            Assert.AreEqual(((Pitch)"Db2").Value, 37);
            Assert.AreEqual(((Pitch)"D2").Value, 38);
            Assert.AreEqual(((Pitch)"Eb2").Value, 39);
            Assert.AreEqual(((Pitch)"E2").Value, 40);
            Assert.AreEqual(((Pitch)"F2").Value, 41);
            Assert.AreEqual(((Pitch)"Gb2").Value, 42);
            Assert.AreEqual(((Pitch)"G2").Value, 43);
            Assert.AreEqual(((Pitch)"Ab2").Value, 44);
            Assert.AreEqual(((Pitch)"A2").Value, 45);
            Assert.AreEqual(((Pitch)"Bb2").Value, 46);
            Assert.AreEqual(((Pitch)"B2").Value, 47);
            Assert.AreEqual(((Pitch)"C3").Value, 48);
            Assert.AreEqual(((Pitch)"Db3").Value, 49);
            Assert.AreEqual(((Pitch)"D3").Value, 50);
            Assert.AreEqual(((Pitch)"Eb3").Value, 51);
            Assert.AreEqual(((Pitch)"E3").Value, 52);
            Assert.AreEqual(((Pitch)"F3").Value, 53);
            Assert.AreEqual(((Pitch)"Gb3").Value, 54);
            Assert.AreEqual(((Pitch)"G3").Value, 55);
            Assert.AreEqual(((Pitch)"Ab3").Value, 56);
            Assert.AreEqual(((Pitch)"A3").Value, 57);
            Assert.AreEqual(((Pitch)"Bb3").Value, 58);
            Assert.AreEqual(((Pitch)"B3").Value, 59);
            Assert.AreEqual(((Pitch)"C4").Value, 60);
            Assert.AreEqual(((Pitch)"Db4").Value, 61);
            Assert.AreEqual(((Pitch)"D4").Value, 62);
            Assert.AreEqual(((Pitch)"Eb4").Value, 63);
            Assert.AreEqual(((Pitch)"E4").Value, 64);
            Assert.AreEqual(((Pitch)"F4").Value, 65);
            Assert.AreEqual(((Pitch)"Gb4").Value, 66);
            Assert.AreEqual(((Pitch)"G4").Value, 67);
            Assert.AreEqual(((Pitch)"Ab4").Value, 68);
            Assert.AreEqual(((Pitch)"A4").Value, 69);
            Assert.AreEqual(((Pitch)"Bb4").Value, 70);
            Assert.AreEqual(((Pitch)"B4").Value, 71);
            Assert.AreEqual(((Pitch)"C5").Value, 72);
            Assert.AreEqual(((Pitch)"Db5").Value, 73);
            Assert.AreEqual(((Pitch)"D5").Value, 74);
            Assert.AreEqual(((Pitch)"Eb5").Value, 75);
            Assert.AreEqual(((Pitch)"E5").Value, 76);
            Assert.AreEqual(((Pitch)"F5").Value, 77);
            Assert.AreEqual(((Pitch)"Gb5").Value, 78);
            Assert.AreEqual(((Pitch)"G5").Value, 79);
            Assert.AreEqual(((Pitch)"Ab5").Value, 80);
            Assert.AreEqual(((Pitch)"A5").Value, 81);
            Assert.AreEqual(((Pitch)"Bb5").Value, 82);
            Assert.AreEqual(((Pitch)"B5").Value, 83);
            Assert.AreEqual(((Pitch)"C6").Value, 84);
            Assert.AreEqual(((Pitch)"Db6").Value, 85);
            Assert.AreEqual(((Pitch)"D6").Value, 86);
            Assert.AreEqual(((Pitch)"Eb6").Value, 87);
            Assert.AreEqual(((Pitch)"E6").Value, 88);
            Assert.AreEqual(((Pitch)"F6").Value, 89);
            Assert.AreEqual(((Pitch)"Gb6").Value, 90);
            Assert.AreEqual(((Pitch)"G6").Value, 91);
            Assert.AreEqual(((Pitch)"Ab6").Value, 92);
            Assert.AreEqual(((Pitch)"A6").Value, 93);
            Assert.AreEqual(((Pitch)"Bb6").Value, 94);
            Assert.AreEqual(((Pitch)"B6").Value, 95);
            Assert.AreEqual(((Pitch)"C7").Value, 96);
            Assert.AreEqual(((Pitch)"Db7").Value, 97);
            Assert.AreEqual(((Pitch)"D7").Value, 98);
            Assert.AreEqual(((Pitch)"Eb7").Value, 99);
            Assert.AreEqual(((Pitch)"E7").Value, 100);
            Assert.AreEqual(((Pitch)"F7").Value, 101);
            Assert.AreEqual(((Pitch)"Gb7").Value, 102);
            Assert.AreEqual(((Pitch)"G7").Value, 103);
            Assert.AreEqual(((Pitch)"Ab7").Value, 104);
            Assert.AreEqual(((Pitch)"A7").Value, 105);
            Assert.AreEqual(((Pitch)"Bb7").Value, 106);
            Assert.AreEqual(((Pitch)"B7").Value, 107);
            Assert.AreEqual(((Pitch)"C8").Value, 108);
            Assert.AreEqual(((Pitch)"Db8").Value, 109);
            Assert.AreEqual(((Pitch)"D8").Value, 110);
            Assert.AreEqual(((Pitch)"Eb8").Value, 111);
            Assert.AreEqual(((Pitch)"E8").Value, 112);
            Assert.AreEqual(((Pitch)"F8").Value, 113);
            Assert.AreEqual(((Pitch)"Gb8").Value, 114);
            Assert.AreEqual(((Pitch)"G8").Value, 115);
            Assert.AreEqual(((Pitch)"Ab8").Value, 116);
            Assert.AreEqual(((Pitch)"A8").Value, 117);
            Assert.AreEqual(((Pitch)"Bb8").Value, 118);
            Assert.AreEqual(((Pitch)"B8").Value, 119);
            Assert.AreEqual(((Pitch)"C9").Value, 120);
            Assert.AreEqual(((Pitch)"Db9").Value, 121);
            Assert.AreEqual(((Pitch)"D9").Value, 122);
            Assert.AreEqual(((Pitch)"Eb9").Value, 123);
            Assert.AreEqual(((Pitch)"E9").Value, 124);
            Assert.AreEqual(((Pitch)"F9").Value, 125);
            Assert.AreEqual(((Pitch)"Gb9").Value, 126);
            Assert.AreEqual(((Pitch)"G9").Value, 127);
        }
        
        [TestMethod]
        public void Pitch_CastFromString_Sharps()
        {
            Assert.AreEqual(((Pitch)"C-1").Value, 0);
            Assert.AreEqual(((Pitch)"C#-1").Value, 1);
            Assert.AreEqual(((Pitch)"D-1").Value, 2);
            Assert.AreEqual(((Pitch)"D#-1").Value, 3);
            Assert.AreEqual(((Pitch)"E-1").Value, 4);
            Assert.AreEqual(((Pitch)"F-1").Value, 5);
            Assert.AreEqual(((Pitch)"F#-1").Value, 6);
            Assert.AreEqual(((Pitch)"G-1").Value, 7);
            Assert.AreEqual(((Pitch)"G#-1").Value, 8);
            Assert.AreEqual(((Pitch)"A-1").Value, 9);
            Assert.AreEqual(((Pitch)"A#-1").Value, 10);
            Assert.AreEqual(((Pitch)"B-1").Value, 11);
            Assert.AreEqual(((Pitch)"C0").Value, 12);
            Assert.AreEqual(((Pitch)"C#0").Value, 13);
            Assert.AreEqual(((Pitch)"D0").Value, 14);
            Assert.AreEqual(((Pitch)"D#0").Value, 15);
            Assert.AreEqual(((Pitch)"E0").Value, 16);
            Assert.AreEqual(((Pitch)"F0").Value, 17);
            Assert.AreEqual(((Pitch)"F#0").Value, 18);
            Assert.AreEqual(((Pitch)"G0").Value, 19);
            Assert.AreEqual(((Pitch)"G#0").Value, 20);
            Assert.AreEqual(((Pitch)"A0").Value, 21);
            Assert.AreEqual(((Pitch)"A#0").Value, 22);
            Assert.AreEqual(((Pitch)"B0").Value, 23);
            Assert.AreEqual(((Pitch)"C1").Value, 24);
            Assert.AreEqual(((Pitch)"C#1").Value, 25);
            Assert.AreEqual(((Pitch)"D1").Value, 26);
            Assert.AreEqual(((Pitch)"D#1").Value, 27);
            Assert.AreEqual(((Pitch)"E1").Value, 28);
            Assert.AreEqual(((Pitch)"F1").Value, 29);
            Assert.AreEqual(((Pitch)"F#1").Value, 30);
            Assert.AreEqual(((Pitch)"G1").Value, 31);
            Assert.AreEqual(((Pitch)"G#1").Value, 32);
            Assert.AreEqual(((Pitch)"A1").Value, 33);
            Assert.AreEqual(((Pitch)"A#1").Value, 34);
            Assert.AreEqual(((Pitch)"B1").Value, 35);
            Assert.AreEqual(((Pitch)"C2").Value, 36);
            Assert.AreEqual(((Pitch)"C#2").Value, 37);
            Assert.AreEqual(((Pitch)"D2").Value, 38);
            Assert.AreEqual(((Pitch)"D#2").Value, 39);
            Assert.AreEqual(((Pitch)"E2").Value, 40);
            Assert.AreEqual(((Pitch)"F2").Value, 41);
            Assert.AreEqual(((Pitch)"F#2").Value, 42);
            Assert.AreEqual(((Pitch)"G2").Value, 43);
            Assert.AreEqual(((Pitch)"G#2").Value, 44);
            Assert.AreEqual(((Pitch)"A2").Value, 45);
            Assert.AreEqual(((Pitch)"A#2").Value, 46);
            Assert.AreEqual(((Pitch)"B2").Value, 47);
            Assert.AreEqual(((Pitch)"C3").Value, 48);
            Assert.AreEqual(((Pitch)"C#3").Value, 49);
            Assert.AreEqual(((Pitch)"D3").Value, 50);
            Assert.AreEqual(((Pitch)"D#3").Value, 51);
            Assert.AreEqual(((Pitch)"E3").Value, 52);
            Assert.AreEqual(((Pitch)"F3").Value, 53);
            Assert.AreEqual(((Pitch)"F#3").Value, 54);
            Assert.AreEqual(((Pitch)"G3").Value, 55);
            Assert.AreEqual(((Pitch)"G#3").Value, 56);
            Assert.AreEqual(((Pitch)"A3").Value, 57);
            Assert.AreEqual(((Pitch)"A#3").Value, 58);
            Assert.AreEqual(((Pitch)"B3").Value, 59);
            Assert.AreEqual(((Pitch)"C4").Value, 60);
            Assert.AreEqual(((Pitch)"C#4").Value, 61);
            Assert.AreEqual(((Pitch)"D4").Value, 62);
            Assert.AreEqual(((Pitch)"D#4").Value, 63);
            Assert.AreEqual(((Pitch)"E4").Value, 64);
            Assert.AreEqual(((Pitch)"F4").Value, 65);
            Assert.AreEqual(((Pitch)"F#4").Value, 66);
            Assert.AreEqual(((Pitch)"G4").Value, 67);
            Assert.AreEqual(((Pitch)"G#4").Value, 68);
            Assert.AreEqual(((Pitch)"A4").Value, 69);
            Assert.AreEqual(((Pitch)"A#4").Value, 70);
            Assert.AreEqual(((Pitch)"B4").Value, 71);
            Assert.AreEqual(((Pitch)"C5").Value, 72);
            Assert.AreEqual(((Pitch)"C#5").Value, 73);
            Assert.AreEqual(((Pitch)"D5").Value, 74);
            Assert.AreEqual(((Pitch)"D#5").Value, 75);
            Assert.AreEqual(((Pitch)"E5").Value, 76);
            Assert.AreEqual(((Pitch)"F5").Value, 77);
            Assert.AreEqual(((Pitch)"F#5").Value, 78);
            Assert.AreEqual(((Pitch)"G5").Value, 79);
            Assert.AreEqual(((Pitch)"G#5").Value, 80);
            Assert.AreEqual(((Pitch)"A5").Value, 81);
            Assert.AreEqual(((Pitch)"A#5").Value, 82);
            Assert.AreEqual(((Pitch)"B5").Value, 83);
            Assert.AreEqual(((Pitch)"C6").Value, 84);
            Assert.AreEqual(((Pitch)"C#6").Value, 85);
            Assert.AreEqual(((Pitch)"D6").Value, 86);
            Assert.AreEqual(((Pitch)"D#6").Value, 87);
            Assert.AreEqual(((Pitch)"E6").Value, 88);
            Assert.AreEqual(((Pitch)"F6").Value, 89);
            Assert.AreEqual(((Pitch)"F#6").Value, 90);
            Assert.AreEqual(((Pitch)"G6").Value, 91);
            Assert.AreEqual(((Pitch)"G#6").Value, 92);
            Assert.AreEqual(((Pitch)"A6").Value, 93);
            Assert.AreEqual(((Pitch)"A#6").Value, 94);
            Assert.AreEqual(((Pitch)"B6").Value, 95);
            Assert.AreEqual(((Pitch)"C7").Value, 96);
            Assert.AreEqual(((Pitch)"C#7").Value, 97);
            Assert.AreEqual(((Pitch)"D7").Value, 98);
            Assert.AreEqual(((Pitch)"D#7").Value, 99);
            Assert.AreEqual(((Pitch)"E7").Value, 100);
            Assert.AreEqual(((Pitch)"F7").Value, 101);
            Assert.AreEqual(((Pitch)"F#7").Value, 102);
            Assert.AreEqual(((Pitch)"G7").Value, 103);
            Assert.AreEqual(((Pitch)"G#7").Value, 104);
            Assert.AreEqual(((Pitch)"A7").Value, 105);
            Assert.AreEqual(((Pitch)"A#7").Value, 106);
            Assert.AreEqual(((Pitch)"B7").Value, 107);
            Assert.AreEqual(((Pitch)"C8").Value, 108);
            Assert.AreEqual(((Pitch)"C#8").Value, 109);
            Assert.AreEqual(((Pitch)"D8").Value, 110);
            Assert.AreEqual(((Pitch)"D#8").Value, 111);
            Assert.AreEqual(((Pitch)"E8").Value, 112);
            Assert.AreEqual(((Pitch)"F8").Value, 113);
            Assert.AreEqual(((Pitch)"F#8").Value, 114);
            Assert.AreEqual(((Pitch)"G8").Value, 115);
            Assert.AreEqual(((Pitch)"G#8").Value, 116);
            Assert.AreEqual(((Pitch)"A8").Value, 117);
            Assert.AreEqual(((Pitch)"A#8").Value, 118);
            Assert.AreEqual(((Pitch)"B8").Value, 119);
            Assert.AreEqual(((Pitch)"C9").Value, 120);
            Assert.AreEqual(((Pitch)"C#9").Value, 121);
            Assert.AreEqual(((Pitch)"D9").Value, 122);
            Assert.AreEqual(((Pitch)"D#9").Value, 123);
            Assert.AreEqual(((Pitch)"E9").Value, 124);
            Assert.AreEqual(((Pitch)"F9").Value, 125);
            Assert.AreEqual(((Pitch)"F#9").Value, 126);
            Assert.AreEqual(((Pitch)"G9").Value, 127);
        }
    }
}
