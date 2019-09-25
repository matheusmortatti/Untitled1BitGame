pico-8 cartridge // http://www.pico-8.com
version 18
__lua__

export("1bitSheet.png")

palt(1, true)
palt(0, false)

__gfx__
11000001111111111100000111111111110700110000000007777770111111111111111111111111007777000000000000000000111111110000000000000000
10077701110000011007770110001111110770110777777070000007111111111111111111111111070000700000007777000000110000110000000000000000
10777701100777011077770100700000110770117000000770000007111111111111111111000011700000070000777007770000100770010000000000000000
10770701107777011077070177777777110770117007700700000000100000011111111110088001707007070007007007007000107007010000000000000000
10777700107707011077770170777770100770017770077777700777008888000000000000800800700000070077007007007700100700010000000000000000
10000077107777001000000000700000107777017007700770077007080000800888888008000080700000070707007007007070110770110000000000000000
17777007177770771777707710001111100700017000000770000007080000808000000808088080070000707007007007007007110000110000000000000000
17007007170070071700700711111111110770117777777777777777008888000888888000800800007777007007007007007007111111110000000000000000
11111111007777000000000088888000000000000000000000000000000000000000000000000000000000007007007007007007000000000000000000000000
11111111070000700000000000808088888880800000000000000000000000000000000000000000000000007007007007007007000000000000000000000000
00000111007777000077700088888008008080880000000000000000000000000000000000000000000000007007007007007007000000000000000000000000
07770000070000700700007000088088888880080000000000000000000000000000000000000000000000007007007007007007000000000000000000000000
07077777777777777700777700008080000880880000000000000000000000000000000000000000000000007007007007007007000000000000000000000000
07770707777777777777077788888088888880800000000000000000000000000000000000000000000000007007007007007007000000000000000000000000
00000000077777700077777080000008800000880000000000000000000000000000000000000000000000007007007007007007000000000000000000000000
11111111007777000000000088888888888888880000000000000000000000000000000000000000000000007777777777777777000000000000000000000000
11000001100777011100000110077701110000011111111111111111111111111111111100000000000000000000000000000000000000000000000000000000
10077701107070011007770110777001100777011100000111100000111000011111111100000000000000000000000000000000000000000000000000000000
10770701107777011077070110777701107707011007770111007770000077001000000000000000000000000000000000000000000000000000000000000000
10777701100000011077770110000001107777011077070111077070077077700077077000000000000000000000000000000000000000000000000000000000
10000001007777001000000100777700100000011077770111077770007070700007077700000000000000000000000000000000000000000000000000000000
10777701070000701077770107000070107777011000000100000000007077700007077700000000000000000000000000000000000000000000000000000000
10700701000110001007700100011000100770011077770100777701077000000077077700000000000000000000000000000000000000000000000000000000
10000001111111111100001111111111110000111000000100000001000011110000000000000000000000000000000000000000000000000000000000000000
000aa000007777000000000011111111111111111000100011111111111111111107070100770000000000000000000000000000000000000000000000000000
00a00a00070000700007770000000111111111110070007011100011111000111007870077007700000000000000000000000000000000000000000000000000
0a0000a0700700070070007099990011100001110700800710008000100080001077077000000000000000000000000000000000000000000000000000000000
0a0000a0700700070007770090909001009900010778087700780870107707701000800000770077000000000000000000000000000000000000000000000000
00a00a00700070070070007090090900090999000700800707708077100787001110001100007700000000000000000000000000000000000000000000000000
00066000700007070777777709907770900977700000000000700070110707011111111177000000000000000000000000000000000000000000000000000000
00077000070000700077777007000007999000071111111110000000110000011111111100777007000000000000000000000000000000000000000000000000
00066000007777000007770000777770007777701111111111111111111111111111111100000000000000000000000000000000000000000000000000000000
00000000007700000077000000077000000077777700077770000000077777700000000000000000000000000000000011077007700111110000000000000000
07007000070770707707700000700700077700000077700007770077070000707777777777777777077777777777777011077770777001110000000000000000
70700007707707700770000000777700700000000000000000007707070000707000000000000007070000000000007000077007000770010000000000000000
07000000770000000000000000707700700000000000000000000007070000707000000000000007070000000000007007777000700007700000000000000000
00000700000007000000770000707700070000000000000000000007070000707000000000000007070000000000007007070007000000700000000000000000
00007070077070700007700700770700070000000000000000000070070000707000000000000007070000000000007007007000700000700000000000000000
07000700707077700000707000770700070000000000000000000070070000707777777777777777070000777700007007000007000000700000000000000000
00000000770007700000000000777700070000000000000000000070070000700700070007000700070000700700007007000007700000700000000000000000
00000000077077700777000000777700070000000000000000000070070000700000000007000070070000700700007007000770077000700000000000000000
00000700777077707777700000707700700000000000000000000070070000707777777707000070070000777700007007077000000770700000000000000000
00000000770077000777000000777700700000000000000000000007070000700000000007000070070000000000007007700000000007700000000000000000
00700000000777000000000000770700700000000000000000000007070000700000000007000070070000000000007000707770077707000000000000000000
00000000770000000000777000770700700000000000000000000007070000700000000007000070070000000000007010707070070707010000000000000000
00000000777077700007777700770700070000000000000000000070070000700000000007000070070000000000007010707070077707010000000000000000
70007700777077000000777000777700070000000000000000000070070000707777777707000070077777777777777010707070000007010000000000000000
70000000000000000000000000777700070000000000000000000070077777700700070007000070070007000700007010777777777777010000000000000000
00000000000000000770077000777700700000000000000000000007070070700700070000000000070007000700007000000000000000000000000000000000
00000077770000007000700700707070700000000000000000000007077777707777777700000000077777777777777000007000000000000000000000000000
00077700007770007007000700777070700000000000000000000007070700700070007007770000070000700070007000000000000000000000000000000000
00770707007077000070007000707070070000000000000000000070077777700070007007077777070000700070007000000070000000000000000000000000
07007000000700700770070000707700070000000000000000000070070070707777777707770707077777777777777000000000000000000000000000000000
70700070070007077007700700777700007000000000000000000007077777707000700000000000070070007000707000007000000000000000000000000000
77007700007700777007000707777770000777000007777700007770070707007000700000000000070070007000707000000000000000000000000000000000
70007000000700700770077077707077000000777770000077770000007070707777777700000000077777777777777000000070000000000000000000000000
07000070700000701111111100000000070007000770770000000000000000000000000000000000000000000000000000000000000000000000000000000000
70707007000707071111111107000700077077007770077000000900007077700000000000000000000000000000000000000000000000000000000000000000
77070700707070770000000107000700077077007707707009009090077077700000000000000000000000000000000000000000000000000000000000000000
00777707707777000770070100000000000000000077000090900900000070000000000000000000000000000000000000000000000000000000000000000000
00070077770070007007770100000000007000707070077009000000077000700000000000000000000000000000000000000000000000000000000000000000
00700700007007000700070100700070007707707700777000000700077707700000000000000000000000000000000000000000000000000000000000000000
07077070070770700077700100700070007707700770770007000000077707000000000000000000000000000000000000000000000000000000000000000000
07770007700077701000001100000000000000000000000007000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
67450000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
__gff__
00080000000000080000000000000000000000000000000000000000000000000a000000000000000000000000000000000000000000000800000000000000000105010501010105050505050d0500000105010501010105050505050505000005050d0501010105050105050101000005050101010501050101010101010000
0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
__map__
5a5858585858490b0c4858585858584360616061606160616061606100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
436868686868681b1c6868686868685370717071707170717071707100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
5343000000000000000000000000005300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
6353000000000000000000000000006300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0063004c4d000000370007000000414100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4100005c5d000000000000000000774100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4176004252000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
76410001420000206200004c4d00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
75000000000000000000115c5d00606100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
7500000000000000000000000000707100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
4100000000000000000000000000004100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
0041000000000000000000000000414100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
6061000000000076606176000000606100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
7071000000000076707176000000707100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
6061000000000000767600000000606100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
7071415141774177414151414141707100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
