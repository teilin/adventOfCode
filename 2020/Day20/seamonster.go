package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
	"strings"
)

type edge struct {
	item0 rune
	item1 rune
	item2 rune
	item3 rune
	item4 rune
	item5 rune
	item6 rune
	item7 rune
	item8 rune
	item9 rune
}

// Constants
const (
	Rows    int = 10
	Columns int = 10
)

var (
	allTiles map[int][][]rune = make(map[int][][]rune)
)

func readInput(inputFile string) {
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	tileNo := 0
	var tiles [][]rune = make([][]rune, 0)
	for scanner.Scan() {
		line := scanner.Text()
		if line != "" {
			if strings.Contains(line, "Tile") {
				header := strings.Split(line, " ")
				tmpNo, _ := strconv.Atoi(header[1][:len(header[1])-1])
				tileNo = tmpNo
			} else {
				tiles = append(tiles, []rune(line))
			}
		} else {
			allTiles[tileNo] = tiles
			tiles = make([][]rune, 0)
		}
	}
}

func edgeToType(e []rune) edge {
	var tmp edge
	tmp.item0 = e[0]
	tmp.item1 = e[1]
	tmp.item2 = e[2]
	tmp.item3 = e[3]
	tmp.item4 = e[4]
	tmp.item5 = e[5]
	tmp.item6 = e[6]
	tmp.item7 = e[7]
	tmp.item8 = e[8]
	tmp.item9 = e[9]
	return tmp
}

func part1() int {
	for tileNo, tile := range allTiles {
		var left []rune = make([]rune, 0)
		var right []rune = make([]rune, 0)
		var top []rune = make([]rune, 0)
		var bottom []rune = make([]rune, 0)
		for r := 0; r < Rows; r++ {
			left = append(left, tile[r][0])
			right = append(right, tile[r][Rows-1])
		}
		for c := 0; c < Columns; c++ {
			top = append(top, tile[0][c])
			bottom = append(bottom, tile[Rows-1][c])
		}
		fmt.Println(tileNo)
	}
	return 0
}

func main() {
	readInput(os.Args[1])
	produktOfEdgeTiles := part1()
	fmt.Println(produktOfEdgeTiles)
}
