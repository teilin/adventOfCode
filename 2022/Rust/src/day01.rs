use std::fs::File;
use std::io::prelude::*;

pub fn part1(path: &str) {
    let mut file = File::open(path).expect("File not found");
    let mut data = String::new();
    file.read_to_string(&mut data).expect("Error while reading file");
    println!("{}", data);
}