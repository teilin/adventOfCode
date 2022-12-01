package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
	"strings"
)

// instruction
type instruction struct {
	operation string
	argument  int
}

func readBootCode(instructions *[]instruction, inputFile string) {
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := strings.Split(scanner.Text(), " ")
		var ins instruction
		ins.operation = line[0]
		ins.argument, _ = strconv.Atoi(strings.TrimSpace(line[1]))
		*instructions = append(*instructions, ins)
	}
}

func part1(instructions *[]instruction, acc *int) {
	counter := make([]int, len(*instructions))
	index := 0
	for true {
		ins := (*instructions)[index]
		if counter[index] >= 1 {
			break
		}
		if ins.operation == "nop" {
			counter[index]++
			index++
		} else if ins.operation == "acc" {
			*acc += ins.argument
			index++
		} else if ins.operation == "jmp" {
			counter[index]++
			index += ins.argument
		}
	}
}

func runProgram(instructions *[]instruction, acc *int) bool {
	*acc = 0
	counter := make([]int, len(*instructions))
	index := 0
	for index < len(*instructions) {
		ins := (*instructions)[index]
		if counter[index] >= 1 {
			return false
		}
		if ins.operation == "nop" {
			counter[index]++
			index++
		} else if ins.operation == "acc" {
			*acc += ins.argument
			index++
		} else if ins.operation == "jmp" {
			counter[index]++
			index += ins.argument
		}
	}
	return true
}

func swapOperations(instruction instruction) instruction {
	if instruction.operation == "nop" {
		instruction.operation = "jmp"
	} else if instruction.operation == "jmp" {
		instruction.operation = "nop"
	}
	return instruction
}

func checkSwitch(instructions *[]instruction, instructionIndexSwitched int, acc *int) bool {
	(*instructions)[instructionIndexSwitched] = swapOperations((*instructions)[instructionIndexSwitched])
	if runProgram(instructions, acc) {
		return true
	} else {
		(*instructions)[instructionIndexSwitched] = swapOperations((*instructions)[instructionIndexSwitched])
		return false
	}
}

func part2(instructions *[]instruction, acc *int) {
	index := 0
	for index < len(*instructions)-1 {
		if checkSwitch(instructions, index, acc) == false {
			index++
		} else {
			break
		}
	}
}

func main() {
	var instructions []instruction
	readBootCode(&instructions, "input.txt")
	accumulator := 0
	part1(&instructions, &accumulator) // Solution: 1331
	fmt.Println(accumulator)
	part2(&instructions, &accumulator) // Solution: 1121
	fmt.Println(accumulator)
}
