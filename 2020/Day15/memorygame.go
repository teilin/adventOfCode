package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"strings"
)

var (
	memoryGame map[int64][]int64 = make(map[int64][]int64)
)

func readInout(inputFile string) (int64, int64) {
	file, err := ioutil.ReadFile(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	strArray := strings.Split(string(file), ",")
	turn := int64(1)
	lastSpoken := int64(0)
	memoryGame = make(map[int64][]int64)
	for _, str := range strArray {
		i, _ := strconv.Atoi(str)
		lastSpoken = int64(i)
		memoryGame[lastSpoken] = append(memoryGame[lastSpoken], turn)
		turn++
	}
	return turn, lastSpoken
}

func playGame(maxRounds int64, turn int64, lastSpoken int64) int64 {
	for turn <= maxRounds {
		prevTurns := memoryGame[lastSpoken]
		if len(prevTurns) == 1 && prevTurns[len(prevTurns)-1] == turn-1 {
			memoryGame[lastSpoken] = append(memoryGame[lastSpoken], turn-1)
			lastSpoken = 0
		} else if prevTurns == nil {
			memoryGame[lastSpoken] = append(memoryGame[lastSpoken], turn-1)
			lastSpoken = 0
		} else {
			memoryGame[lastSpoken] = append(memoryGame[lastSpoken], turn-1)
			lastSpoken = memoryGame[lastSpoken][len(memoryGame[lastSpoken])-1] - memoryGame[lastSpoken][len(memoryGame[lastSpoken])-2]
		}
		turn++
	}
	return lastSpoken
}

func part1(inputFile string) int64 {
	turn, lastSpoken := readInout(inputFile)
	return playGame(2020, turn, lastSpoken)
}

func part2(inputFile string) int64 {
	turn, lastSpoken := readInout(inputFile)
	return playGame(30000000, turn, lastSpoken)
}

func main() {
	num2020 := part1(os.Args[1])
	fmt.Println(num2020)
	num2020 = part2(os.Args[1])
	fmt.Println(num2020)
}
