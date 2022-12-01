package main

import (
	"bufio"
	"fmt"
	"log"
	"math"
	"os"
	"strconv"
)

func readInputPublicKeys(inputFile string) (int64, int64) {
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	publicKeys := make([]int64, 0)
	for scanner.Scan() {
		publicKey, _ := strconv.Atoi(scanner.Text())
		publicKeys = append(publicKeys, int64(publicKey))
	}
	return publicKeys[0], publicKeys[1]
}

func pow(x, y, z int) int {
	p := math.Pow(float64(x), float64(y))
	return int(p) % z
}

func transform(x int, sz int) int {
	return pow(x, sz, 20201227)
}

func transform2(x, y int64) int64 {
	return pow2(x, y, int64(20201227))
}

func pow2(x, y, z int64) int64 {
	var p int64 = 1
	var i int64 = 1
	for i = 1; i <= y; i++ {
		p *= x
	}
	return p % z
}

func part1(pk1, pk2 int64) int64 {
	/*loopNumber1 := 0
	for transform(7, loopNumber1) != pk1 {
		loopNumber1++
	}
	loopNumber2 := 0
	for transform(7, loopNumber2) != pk2 {
		loopNumber2++
	}
	encryption := transform(pk1, loopNumber2)
	if encryption != transform(pk2, loopNumber1) {
		panic("Wrong encryption key")
	}
	return encryption*/
	var ln1 int64 = 0
	for transform2(7, ln1) != pk1 {
		ln1++
	}
	var ln2 int64 = 0
	for transform2(7, ln2) != pk2 {
		ln2++
	}
	encryption := transform2(pk1, ln2)
	encryption2 := transform2(pk2, ln1)
	if encryption != encryption2 {
		panic("Wrong encryption key")
	}
	return encryption
}

func main() {
	pk1, pk2 := readInputPublicKeys(os.Args[1])
	encryptionKey := part1(pk1, pk2)
	fmt.Println(encryptionKey)
}
