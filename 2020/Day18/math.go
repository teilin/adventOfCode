package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
)

// Stack type
type Stack []EquationElement

// IsEmpty check for stack
func (s *Stack) IsEmpty() bool {
	return len(*s) == 0
}

// Push to stack
func (s *Stack) Push(elm EquationElement) {
	*s = append(*s, elm)
}

// Pop value from stack
func (s *Stack) Pop() (EquationElement, bool) {
	if s.IsEmpty() {
		return EquationElement(0), false
	}
	index := len(*s) - 1
	element := (*s)[index]
	*s = (*s)[:index]
	return element, true
}

// Equation type
type Equation string

// EquationElement type
type EquationElement int

func readInput(inputfile string) []Equation {
	file, err := os.Open(inputfile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	var equations []Equation = make([]Equation, 0)
	for scanner.Scan() {
		eq := Equation(scanner.Text())
		equations = append(equations, eq)
	}
	return equations
}

func findEndExp(exp *Equation, start int) int {
	counter := 0
	for i := start; i < len(*exp); i++ {
		if string((*exp)[i]) == "(" {
			counter++
		}
		if string((*exp)[i]) == ")" {
			counter--
		}
		if counter == 0 {
			return i
		}
	}
	return len(*exp) - 1
}

func (n *Equation) solve(start int, end int) int {
	prevValue := 0
	value := 0
	//var stack Stack
	for i := start; i < end-1; i++ {
		elm := string((*n)[i])
		if elm != " " {
			if elm == "(" {
				endExp := findEndExp(n, i)
				value = n.solve(i, endExp)
			}

		}
	}
	return ans
}

func part1(equations []Equation) int {
	sum := 0
	for _, eq := range equations {
		sum += eq.solve(0, len(eq))
	}
	return sum
}

func main() {
	equations := readInput(os.Args[1])
	sum := part1(equations)
	fmt.Println(sum)
}
