package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
	"strings"
)

var (
	player1 Stack
	player2 Stack
	seen    []Tuple
)

// Tuple struct
type Tuple struct {
	p1 Card
	p2 Card
}

// Card type
type Card int

// Stack type
type Stack []Card

// IsEmpty check for stack
func (s *Stack) IsEmpty() bool {
	return len(*s) == 0
}

// Push to stack
func (s *Stack) Push(card Card) {
	*s = append(*s, card)
}

// Pop value from stack
func (s *Stack) Pop() (Card, bool) {
	if s.IsEmpty() {
		return Card(0), false
	}
	index := 0
	element := (*s)[index]
	*s = (*s)[index+1:]
	return element, true
}

// Count number of value left
func (s *Stack) Count() int {
	return len(*s)
}

// Contains functions
func Contains(slice []Tuple, elm Tuple) bool {
	for _, a := range slice {
		if a == elm {
			return true
		}
	}
	return false
}

func readInput(inputFile string) {
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	isFirstPlayer := true
	for scanner.Scan() {
		line := scanner.Text()
		if strings.Contains(line, "Player") {
			array := strings.Split(line, " ")
			num, _ := strconv.Atoi(array[1][:len(array[1])-1])
			if num == 2 {
				isFirstPlayer = false
			}
		} else {
			if line != "" {
				card, _ := strconv.Atoi(line)
				if isFirstPlayer {
					player1.Push(Card(card))
				} else {
					player2.Push(Card(card))
				}
			}
		}
	}
}

func readInput2(inputFile string) (Stack, Stack) {
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	isFirstPlayer := true
	var p1, p2 Stack
	for scanner.Scan() {
		line := scanner.Text()
		if strings.Contains(line, "Player") {
			array := strings.Split(line, " ")
			num, _ := strconv.Atoi(array[1][:len(array[1])-1])
			if num == 2 {
				isFirstPlayer = false
			}
		} else {
			if line != "" {
				card, _ := strconv.Atoi(line)
				if isFirstPlayer {
					p1.Push(Card(card))
				} else {
					p2.Push(Card(card))
				}
			}
		}
	}
	return p1, p2
}

func part1() int {
	for player1.IsEmpty() == false && player2.IsEmpty() == false {
		currentP1, _ := player1.Pop()
		currentP2, _ := player2.Pop()

		if currentP1 > currentP2 {
			player1.Push(currentP1)
			player1.Push(currentP2)
		}
		if currentP2 > currentP1 {
			player2.Push(currentP2)
			player2.Push(currentP1)
		}
	}
	var winningDeck []int = make([]int, 0)
	if player1.IsEmpty() {
		for player2.IsEmpty() == false {
			v, _ := player2.Pop()
			winningDeck = append(winningDeck, int(v))
		}
	} else {
		for player1.IsEmpty() == false {
			v, _ := player1.Pop()
			winningDeck = append(winningDeck, int(v))
		}
	}
	score := 0
	for i, c := range winningDeck {
		score += (len(winningDeck) - i) * c
	}
	return score
}

func playGame(p1 Stack, p2 Stack) (bool, Stack) {
	for player1.IsEmpty() == false && player2.IsEmpty() == false {
		currentP1, _ := player1.Pop()
		currentP2, _ := player2.Pop()

		current := Tuple{p1: currentP1, p2: currentP2}
		if Contains(seen, current) {
			return true, p1
		}
		seen = append(seen, current)
		p1wins := false
		if player1.Count() >= int(currentP1) && player2.Count() >= int(currentP2) {
			var newP1 Stack
			copy(newP1, p1)
			var newP2 Stack
			copy(newP2, p2)
			p1wins, _ = playGame(newP1, newP2)
		} else {
			p1wins = currentP1 > currentP2
		}
		if p1wins {
			player1.Push(currentP1)
			player1.Push(currentP2)
		} else {
			player2.Push(currentP2)
			player2.Push(currentP2)
		}
	}
	if p1.IsEmpty() {
		return false, p2
	} else {
		return true, p1
	}
}

func part2(p1 Stack, p2 Stack) int {
	var winning []int = make([]int, 0)
	_, winningDeck := playGame(p1, p2)
	for winningDeck.IsEmpty() == false {
		v, _ := winningDeck.Pop()
		winning = append(winning, int(v))
	}
	score := 0
	for i, c := range winning {
		score += (len(winning) - i) * c
	}
	return score
}

func main() {
	readInput(os.Args[1])
	winnerScore := part1()
	fmt.Println(winnerScore)
	p1, p2 := readInput2(os.Args[1])
	winnerScore = part2(p1, p2)
	fmt.Println(winnerScore)
}
