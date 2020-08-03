# --------------------------------------------------------------------------- #
#
# Copyright (c) 2010 CubeSoft, Inc.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#  http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#
# --------------------------------------------------------------------------- #
require 'rake'
require 'rake/clean'

# --------------------------------------------------------------------------- #
# configuration
# --------------------------------------------------------------------------- #
PROJECT     = "Cube.Note"
BRANCHES    = ["master", "net35"]
FRAMEWORKS  = ["net45", "net35"]
CONFIGS     = ["Release", "Debug"]
PLATFORMS   = ["Any CPU", "x86", "x64"]

# --------------------------------------------------------------------------- #
# build_v1_all
# --------------------------------------------------------------------------- #
desc "Build task for version 1.15.x."
task :build_v1_all => :build_all

# --------------------------------------------------------------------------- #
# clean
# --------------------------------------------------------------------------- #
CLEAN.include(["*.nupkg", "**/bin", "**/obj"])
CLOBBER.include("../packages/cube.*")

# --------------------------------------------------------------------------- #
# default
# --------------------------------------------------------------------------- #
desc "Clean, build, test, and create NuGet packages."
task :default => [:clean] do
    Rake::Task[:build_all].invoke(false)
end

# --------------------------------------------------------------------------- #
# restore
# --------------------------------------------------------------------------- #
desc "Resote NuGet packages in the current branch."
task :restore do
    cmd("nuget restore #{PROJECT}.sln")
end

# --------------------------------------------------------------------------- #
# build
# --------------------------------------------------------------------------- #
desc "Build projects in the current branch."
task :build, [:platform] do |_, e|
    e.with_defaults(:platform => PLATFORMS[0])

    Rake::Task[:restore].execute
    branch = %x(git rev-parse --abbrev-ref HEAD).chomp
    build  = branch.start_with?("netstandard") || branch.start_with?("netcore") ?
             "dotnet build -c Release" :
             "msbuild -v:m -p:Configuration=Release"
    cmd(%(#{build} -p:Platform="#{e.platform}" #{PROJECT}.sln))
end

# --------------------------------------------------------------------------- #
# build_all
# --------------------------------------------------------------------------- #
desc "Build projects in pre-defined branches and platforms."
task :build_all, [:test] do |_, e|
    e.with_defaults(:test => false)
    
    BRANCHES.product(PLATFORMS).each do |bp|
        checkout(bp[0]) do
            Rake::Task[:build].reenable
            Rake::Task[:build].invoke(bp[1])
            Rake::Task[:test].execute if (e.test)
        end
    end
end

# --------------------------------------------------------------------------- #
# test
# --------------------------------------------------------------------------- #
desc "Test projects in the current branch."
task :test do
    cmd("dotnet test -c Release --no-restore --no-build #{PROJECT}.sln")
end

# --------------------------------------------------------------------------- #
# checkout
# --------------------------------------------------------------------------- #
def checkout(branch, &callback)
    cmd("git checkout #{branch}")
    callback.call()
ensure
    cmd("git checkout master")
end

# --------------------------------------------------------------------------- #
# cmd
# --------------------------------------------------------------------------- #
def cmd(args)
    sh("cmd.exe /c #{args}")
end