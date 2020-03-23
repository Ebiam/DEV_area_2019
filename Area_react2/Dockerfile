FROM archlinux/base:latest

RUN (printf "\nen_US.UTF-8 UTF-8\n" >> /etc/locale.gen) && (/usr/bin/locale-gen)
RUN sed -i "/\[multilib\]/,/Include/"'s/^#//' /etc/pacman.conf

RUN pacman -Syy

RUN pacman -S jdk8-openjdk --noconfirm
ENV JAVA_HOME=/usr/lib/jvm/default

RUN pacman -S --noconfirm sudo audit binutils make git fakeroot
RUN useradd builduser -m 
RUN passwd -d builduser
RUN printf 'builduser ALL=(ALL) ALL\n' | tee -a /etc/sudoers
RUN sudo -u builduser bash -c 'cd ~ && git clone https://aur.archlinux.org/android-sdk.git && cd android-sdk && makepkg -si --noconfirm'
RUN sudo -u builduser bash -c 'cd ~ && git clone https://aur.archlinux.org/android-sdk-build-tools.git  && cd android-sdk-build-tools && makepkg -si --noconfirm'
RUN sudo -u builduser bash -c 'cd ~ && git clone https://aur.archlinux.org/android-sdk-platform-tools.git  && cd android-sdk-platform-tools && makepkg -si --noconfirm'

ENV ANDROID_HOME=/opt/android-sdk
ENV PATH=$PATH:$ANDROID_HOME/emulator
ENV PATH=$PATH:$ANDROID_HOME/tools
ENV PATH=$PATH:$ANDROID_HOME/tools/bin
ENV PATH=$PATH:$ANDROID_HOME/platform-tools

RUN yes | sdkmanager --update
RUN yes | sdkmanager --licenses
RUN yes | sdkmanager "build-tools;27.0.3" "platforms;android-27"

ENV PATH /usr/src/app/node_modules/.bin:$PATH

RUN mkdir -p /usr/src/app/android/gradle/wrapper/ \
    && curl -fl https://downloads.gradle.org/distributions/gradle-5.1.1-all.zip \
       -o /usr/src/app/android/gradle/wrapper/gradle-all.zip

RUN pacman -S npm --noconfirm

RUN npm install yarn

COPY package.json /tmp/package.json
RUN cd /tmp && npm install --no-progress --ignore-optional
RUN cp -a /tmp/node_modules /usr/src/app/

COPY . /usr/src/app/
RUN ls /usr/src/app/android/
RUN rm /usr/src/app/android/local.properties ; exit 0
WORKDIR /usr/src/app/android/
#RUN ls android/app/build/outputs/apk/debug/
RUN ./gradlew clean
# CMD ["./gradlew", "assembleRelease"]
RUN ./gradlew assembleRelease
RUN ls
RUN pwd
RUN ls
WORKDIR /usr/src/app/android/app
RUN ls build/
RUN ls build/outputs/apk
RUN ls build/outputs/apk/release/

#FROM node:latest AS builder
#WORKDIR /usr/src/app/myapp

# Copy csproj and restore as distinct layers
#RUN ls
#COPY node_modules ./node_modules
#RUN yarn install
#RUN pwd
#RUN ls
#COPY . ./
#RUN ls
#RUN ls android/app/build/outputs/apk/debug/ #app-debug.apk
#WORKDIR /usr/src/app/myapp/android/
#RUN gradle wrapper --gradle-version 6.2
#RUN chmod 777 gradlew
#RUN ls
#RUN ./gradlew tasks
#RUN ./gradlew clean
#RUN ./gradlew assembleDebug
#RUN ls app/build/outputs/apk/debug/

# Build runtime image
#FROM gradle:latest
#WORKDIR  /usr/src/app/myapp
#COPY --from=builder /usr/src/app/myapp .
#RUN ls
#WORKDIR /usr/src/app/myapp/android/
#RUN gradle wrapper --gradle-version 6.2
#RUN chmod 777 gradlew
#RUN ls
#RUN ./gradlew tasks
#RUN ./gradlew clean
#RUN ./gradlew assembleDebug
#RUN ls app/build/outputs/apk/debug/ # app-debug.apk



