pico-8 cartridge // http://www.pico-8.com
version 18
__lua__

export("1bitSheet.png")

palt(1, true)
palt(0, false)

__gfx__
11000001111111111100000111111111110700110000000007777770111111111111111111111111007777000000000000000000111111110000000000000000
10077701110000011007770110001111110770110777777070000007111111111111111111111111070000700000009999000000110000110000000000000000
10777701100777011077770100700000110770117000000770000007111111111111111111000011700000070000999009990000100770010000000000000000
10770701107777011077070177777777110770117007700700000000100000011111111110088001707007070009009009009000107007010000000000000000
10777700107707011077770170777770100770017770077777700777008888000000000000800800700000070099009009009900100700010000000000000000
10000077107777001000000000700000107777017007700770077007080000800888888008000080700000070909009009009090110770110000000000000000
17777007177770771777707710001111100700017000000770000007080000808000000808088080070000709009009009009009110000110000000000000000
17007007170070071700700711111111110770117777777777777777008888000888888000800800007777009009009009009009111111110000000000000000
11111111007777000000000088888000000000000000000000000000090090900000000009009090000000009009009009009009000000000000000000000000
11111111070000700000000000808088888880800900909009009090099099900900909009909990000000009009009009009009000000000000000000000000
00000111007777000077700088888008008080880990999009909990099909900990999009990990000000009009009009009009000000000000000000000000
07770000070000700700007000088088888880080999099009990990809070780999099080900078000000009009009009009009000000000000000000000000
07077777777777777700777700008080000880888090070880900708800000088090070880000008000000009009009009009009000000000000000000000000
07770707777777777777077788888088888880808000000880000008088888808000000808888880000000009009009009009009000000000000000000000000
00000000077777700077777080000008800000880888888008888880070000700888888007000070000000009009009009009009000000000000000000000000
11111111007777000000000088888888888888880070070000077000000000000007700000000000000000009999999999999999000000000000000000000000
11000001100777011100000110077701110000011111111111111111111111111111111100000000007700111110007700111111000000000000000000000000
10077701107070011007770110777001100777011100000111100000111000011111111100000000997070111110997070111111000000000000000000000000
10770701107777011077070110777701107707011007770111007770000077001000000000000000097770000000097770000000000000000000000000000000
10777701100000011077770110000001107777011077070111077070077077700077077000000000000077777770000077777770000000000000000000000000
10000001007777001000000100777700100000011077770111077770007070700007077700000000111000700007011100700007000000000000000000000000
10777701070000701077770107000070107777011000000100000000007077700007077700000000111100077770011110077770000000001100000000000000
10700701000110001007700100011000100770011077770100777701077000000077077700000000111109090990111110090099007700111100000000000000
10000001111111111100001111111111110000111000000100000001000011110000000000000000111100900090111110990009997070111100000000000000
000aa000007777000000000011111111111111111000100011111111111111111107070100770000100001111111100000111111097770111100000000000000
00a00a00070000700007770000000111111111110070007011100011111000111007870077007700007700111110007700111111000070111100000000000000
0a0000a0700700070070007099990011100001110700800710008000100080001077077000000000997070111110997070000000110700000000000000000000
0a0000a0700700070007770090909001009900010778087700780870107707701000800000770077097770000000097777777770110777777000000000000000
00a00a00700070070070007090090900090999000700800707708077100787001110001100007700000077777770000000700007110070000700000000000000
00066000700007070777777709907770900977700000000000700070110707011111111177000000111000700007011110077770111007777000000000000000
00077000070000700077777007000007999000071111111110000000110000011111111100777007111110077770011110909090111009009000000000000000
00066000007777000007770000777770007777701111111111111111111111111111111100000000111111000900011110090990111099099000000000000000
00000000007700000077000000077000000077777700077770000000077777700000000000000000000000000000000011077007700111117777777777777777
07007000070770707707700000700700077700000077700007770077070000707777777777777777077777777777777011077770777001117700000000000077
70700007707707700770000000777700700000000000000000007707070000707000000000000007070000000000007000077007000770017077777777777707
07000000770000000000000000707700700000000000000000000007070000707000000000000007070000000000007007777000700007707070000000000707
00000700000007000000770000707700070000000000000000000007070000707000000000000007070000000000007007070007000000707070000000000707
00007070077070700007700700770700070000000000000000000070070000707000000000000007070000000000007007007000700000707070000770000707
07000700707077700000707000770700070000000000000000000070070000707777777777777777070000777700007007000007000000707070070000700707
00000000770007700000000000777700070000000000000000000070070000700700070007000700070000700700007007000007700000707070070000700707
00000000077077700777000000777700070000000000000000000070070000700000000007000070070000700700007007000770077000707070000770000707
00000700777077707777700000707700700000000000000000000070070000707777777707000070070000777700007007077000000770707070000000000707
00000000770077000777000000777700700000000000000000000007070000700000000007000070070000000000007007700000000007707070000000000707
00700000000777000000000000770700700000000000000000000007070000700000000007000070070000000000007000707770077707007070777777770707
00000000770000000000777000770700700000000000000000000007070000700000000007000070070000000000007010707070070707017077700000077707
00000000777077700007777700770700070000000000000000000070070000700000000007000070070000000000007010707070077707017700777777770077
70007700777077000000777000777700070000000000000000000070070000707777777707000070077777777777777010707070000007010000700000070000
70000000000000000000000000777700070000000000000000000070077777700700070007000070070007000700007010777777777777010000777777770000
00000000000000000770077000777700700000000000000000000007070070700700070011111111070007000700007000000000000000007777777777777777
00000077770000007000700700707070700000000000000000000007077777707777777700000111077777777777777000007000000000007000000000000007
00077700007770007007000700777070700000000000000000000007070700700070007007770000070000700070007000000000000000007000000000000007
00770707007077000070007000707070070000000000000000000070077777700070007007077777070000700070007000000070000000007000000000000007
07007000000700700770070000707700070000000000000000000070070070707777777707770707077777777777777000000000000000007777777777777777
70700070070007077007700700777700007000000000000000000007077777707000700000000000070070007000707000007000000000007700000000000077
77007700007700777007000707777770000777000007777700007770070707007000700011111111070070007000707000000000000000007700000000000077
70007000000700700770077077707077000000777770000077770000007070707777777711111111077777777777777000000070000000007777777777777777
07000070700000701111111100000000070007000770770000000000000000000000000000000000070007000000000000000000000000007070000000000707
70707007000707071111111107000700077077007770077000000900007077700000000007000700077077000000000000000000000000007070000000000707
77070700707070770000000107000700077077007707707009009090077077700700070007000700077077000000000000000000000000007077777777777707
00777707707777000770070100000000000000000077000090900900000070000000000000000000000000000000000000000000000000007007000000007007
00070077770070007007770100000000007000707070077009000000077000700000000000000000007000700000000000000000000000007007000000007007
00700700007007000700070100700070007707707700777000000700077707700000000000700070007707700000000000000000000000007007777777777007
07077070070770700077700100700070007707700770770007000000077707000070007000700070007707700000000000000000000000007000000000000007
07770007700077701000001100000000000000000000000007000000000000000000000000000000000000000000000000000000000000007777777777777777
35363536350067343400003500353514143614141436000000001414361414141414141414140000141414363514141414141435141435145714145714141414
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
35003600363405363667343634363614140000000000000000000000000000141400000000000000000000003600001414000036000035000000000000000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
35340005003600000000360035000014140000830000000000000000830000000000000000000000000000000000001414000000000036000000000000000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
36350000000000000000000036003414140000000000000616000000000000773400000000000000000000000004000000000000050000000000061600000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00360000006700e40000670000003534140000000000000717000000000000143500000400000000000506160000000000000000008300000000071700000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
34141400006700000000670000273635140000000000006767001400000000343600000000061600000007170000000000000000000000000000000500000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
35000000000000000000000000340035140000000000000000000000000000357700700000071767000000000000000000000000007757000031000031000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
36340000000000002500000500363436140000000000310000310000000000353400000000000000000000000000000000000000005777000000a7a700000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00350000000000240000000000003500140000000000040000000000000000363500000000310500000000000000000000000000005757000000a7a700000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
14360000040000000000000000043600140000000000000000000073000000143600000000000000000000000000000000000000057757000031000031000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
1414000000000000000000000034001414000000a200000000000000000000000000000006160000700400003100000000000000000000000000000000000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
34340000000000000000000004350034140000050000700000000067670000000000000007170000000000000000001414000073000000000000061600000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
35360400000000000000000000360035140000000000000000006706160000000000000467670000000000061600001414000000000000000000071700000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
36000000000000000000000000000036140000000000050000006707170000573400000000000000000067071700001414000400000000000000000000000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
06160006167400000000740616000616140000000000000000000000000000343500000000000000000000000000001414000000000616000000000000000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
07171407179500000000950717140717141414771457141457141414147714353657570000061600000616000014141414141477140717771414000000571414
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0616140616a585c00085b50616140616140616141457141457141414140616361414140000071700000717000014141434141414141414141414000000141414
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0717000717a686000086b607170007171407170000000000000000000007171414000000000000000000000000000014350000a485858585b400000000000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
14000000000000000000000000000014770000000000000000000000000000000000000004000000000000000000003436000095868686869500000000000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
14000000000000000000000005000014340000510000a48585b40000000000000000000000000000000400000070003577000095110000119500000000000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
14000057570000000000005757000014353400000000758686750000000000141400000000000000000000000000003534000095110000119500000000000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
140000575700310000310057570000143635000000007623237600000000001414a700575757a7a7a7a757575700a736350000a594000084b500000000000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
14000000000000000000000000000014773600050000a70000a70000000000061600a7575757a7a7a7a7575757a70077360000a686000086b600006767670014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
1400007300005757575700007000001434000000000031a7a7310000000000071700000000000000000000000000000000000000000000000000006767670014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
14000000000000000000000000000014350000000000a70000a70000730000771400007000000000700000000000000000000000000000000000006767670014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
14a700a700a700a705a700a700a700143600000000007423237400000000005714a7575757000000000000000000000000000000000000102600000000000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
1400a700a700a700a700a700a700a71477a700a700a7a58585b500000000001414a7575757000000000000000500000000000000270000000000000000000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
140000000000000000000000000000141400a700a700a68686b6000000050014140000000000310000000000000000061600000034000000000000a200000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
140000a200000000000000700000000000000073000000a7a7000000000000000000000000000000777700000000000717000000360000050000000000000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
1400000000000000730000040000000000000000000000a7a7000000000000000000000500000000777700000000000616000000000000000000000005000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
1400000000000000000000000000001414000000000000a7a7000000000000141400000000000000310000000000000717000000000000000000000000000014
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
14141414141414141414141414141414141414571414145714141457145714141414141414141414141414141414147777141414141414141414141414141414
__gff__
00080000000000080000000a08000000000500080008000000000000000000000a000000000000000000080000080000000000000000000800000000000800000105010501010105050505050d050a050105010501010105050505050505050505050d0501010105050805050101010105050501010501050000080101010101
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
__map__
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000004a58585858585858585858585858584b00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005968686868686868686868686868685900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005900000000000000000000000000005900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005900000000000000000000000000005900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005900004060610000400060615000005900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005900000070710000000070710000005900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005900000000000000000000000000005900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000590000000000006e6f0000000000005900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000590050000000007e7f0000000000005900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005900000000000040000000000000005900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005900000060610000000060610000005900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005900000070710000000070710000005900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005900000000000000000000000000005900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000000000000000430000000000000000005900000000000000000000000000005900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000000000000000530000000000430000005900004000000000000000000050005900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000000000000043630000000043530000005900000000000000000000000000005900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4141414141414141414141414141414141606141414141414160614141606141606160614353050000606153636061435a5858585858490b004858585858584300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4141000000000000000000000000004141707177756061000070710000707141437170715363436061707163007071534368686868686800006868686868685300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4100000000000000000000000000006061000000007071000000000000000075530000006300537071405000000000635300000000000000000000000000005300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4100000000000000000000000000007071000000000000001300000000000041634300000000634141606100000043006343004c4d000000000000400000436300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4100000000000000000000000000006061000000000000000000000700000000005300000000000041707100000053434153005c5d000000000000000043530000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4100000000000000000000000000007071000000000000000000000000000000006300000000000000000000000063534363004242000000000000000063630000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4100000000000000000000000000006061000000000000000000000000000000000000000000000000004000000000635300000152000000622000000000770000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4100000000000000000000000000007071000000606160616061606100000000000000005000000000000000410000006300000000000000000000000000754300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4100000000000000000000000000006061000000707170717071707100000000000000000000134100000000414100000000000000000000000000000000435300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4100000000000000000000000000007071000000000000000000000000000000000000000000414100000000000000000000000000000000767600000000536300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4100000000000000000000000000006061000000000000000013000000000000000000000000000000001300000000000000005000000076606100000000537700000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4100000000000000000000000000007071000000000700000000004000000041410000000000000050000000414100000043000000000076707101000000637700000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4100000000000000000000000000006061000000000000000000000000000041414100001300000041414100410000430063000000000000767600000000777700000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4143004300000000000000000000437071004300000000000000004300000041417500000000000041004100000000534311110000004350000000000000754300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4353435300000000000000430000534141435300004300000000005343000041606100006061000041434143436061635375754300005300000000000000775300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
5353535343414141414141534143534141536341755300000000416353414141707141757071000041637553537071006375755341415341414175414175416300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000