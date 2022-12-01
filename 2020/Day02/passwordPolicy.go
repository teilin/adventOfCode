package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
	"strings"
)

func countCharInString(inputString string, c string) int {
	num := 0
	for _, char := range inputString {
		if string(char) == c {
			num++
		}
	}
	return num
}

func part1() int {
	file, err := os.Open("./passwordList.txt")
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	count := 0
	for scanner.Scan() {
		line := scanner.Text()
		i := strings.Index(line, "-")
		min, _ := strconv.Atoi(string(line[:i]))
		i2 := strings.Index(line, " ")
		max, _ := strconv.Atoi(string(line[i+1 : i2]))
		i3 := strings.Index(line, ":")
		char := strings.TrimSpace(string(line[i2:i3]))
		pwd := strings.TrimSpace(string(line[i3+1 : len(line)]))

		num := countCharInString(pwd, char)
		if num >= min && num <= max {
			count++
		}
	}
	return count
}

func part2() int {
	file, err := os.Open("./passwordList.txt")
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	count := 0
	for scanner.Scan() {
		line := scanner.Text()
		i := strings.Index(line, "-")
		pos1, _ := strconv.Atoi(string(line[:i]))
		i2 := strings.Index(line, " ")
		pos2, _ := strconv.Atoi(string(line[i+1 : i2]))
		i3 := strings.Index(line, ":")
		char := strings.TrimSpace(string(line[i2:i3]))
		pwd := strings.TrimSpace(string(line[i3+1 : len(line)]))

		charAtPos1 := string(pwd[pos1-1])
		charAtPos2 := string(pwd[pos2-1])

		if (char == charAtPos1 && char != charAtPos2) || (char != charAtPos1 && char == charAtPos2) {
			count++
		}
	}
	return count
}

func main() {
	numValidPasswordsPart1 := part1()

	fmt.Println(numValidPasswordsPart1)

	numValidPasswordsPart2 := part2()
	fmt.Println(numValidPasswordsPart2)
}
