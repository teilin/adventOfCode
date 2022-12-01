package main

import (
	"container/list"
	"fmt"
	"os"
	"strconv"
)

func stringToIntSlice(input string) []int {
	var tmp []int = make([]int, len(input))
	for i, s := range input {
		tmp[i], _ = strconv.Atoi(string(s))
	}
	return tmp
}

func getDesinationNumber(inputNumber []int, pickupNum []int, currentNumber int) (int, int) {
	maxNum, maxIndex := 0, 0
	for i, n := range inputNumber {
		if contains(pickupNum, n) == false {
			if n > maxNum {
				maxIndex = i
				maxNum = n
			}
		}
		if n == currentNumber-1 {
			return n, i
		}
	}
	return maxNum, maxIndex
}

func contains(array []int, num int) bool {
	for _, n := range array {
		if n == num {
			return true
		}
	}
	return false
}

func removeIndex(s []int, index int) []int {
	return append(s[:index], s[index+1:]...)
}

func reOrderSlice(inputNumber []int, pickupNumbers []int, destValue int) []int {
	var newSlice []int = make([]int, 0)
	for _, n := range inputNumber {
		if contains(pickupNumbers, n) == false {
			newSlice = append(newSlice, n)
			if n == destValue {
				for _, m := range pickupNumbers {
					newSlice = append(newSlice, m)
				}
			}
		}
	}
	return newSlice
}

func (l *list.Element) Find(elm int) list.Element {
	for e:=
}

func part1(l *list.List, inputNumber []int, numRounds int) string {
	for _, n := range inputNumber {
		l.PushBack(n)
	}
	//pickUp := make([]int, 3)
	for e := l.Front(); e != nil; e = e.Next() {

	}
	/*var pickUpNumbers []int = make([]int, 3)
	for move := 0; move < numRounds; move++ {
		pickUpNumbers = make([]int, 3)
		currentIndex := move % len(inputNumber)
		pickUpNumbers[0] = inputNumber[(currentIndex+1)%len(inputNumber)]
		pickUpNumbers[1] = inputNumber[(currentIndex+2)%len(inputNumber)]
		pickUpNumbers[2] = inputNumber[(currentIndex+3)%len(inputNumber)]
		//inputNumber = removeIndex(inputNumber, (currentIndex+1)%len(inputNumber))
		//inputNumber = removeIndex(inputNumber, (currentIndex+1)%len(inputNumber))
		//inputNumber = removeIndex(inputNumber, (currentIndex+1)%len(inputNumber))
		destNum, _ := getDesinationNumber(inputNumber, pickUpNumbers, inputNumber[currentIndex])
		for contains(pickUpNumbers, destNum) == true {
			destNum, _ = getDesinationNumber(inputNumber, pickUpNumbers, destNum)
		}
		inputNumber = reOrderSlice(inputNumber, pickUpNumbers, destNum)
	}*/
	return ""
}

func part2(inputNumber []int, numRounds int) string {
	return ""
}

func main() {
	l := list.New()
	inputArray := stringToIntSlice(os.Args[1])
	numRounds, _ := strconv.Atoi(os.Args[2])
	num := part1(&l, inputArray, numRounds)
	fmt.Println(num)
	num = part2(inputArray, numRounds)
	fmt.Println(num)
}
